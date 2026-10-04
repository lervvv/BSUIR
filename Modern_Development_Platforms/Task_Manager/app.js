const express = require('express');
const session = require('express-session');
const SQLiteStore = require('connect-sqlite3')(session);
const multer = require('multer');
const path = require('path');
const fs = require('fs');
const sqlite3 = require('sqlite3').verbose();
const bcrypt = require('bcryptjs');
const crypto = require('crypto');

const app = express();
const PORT = 3000;

// ==========================================
// 1. ПРОВЕРКА И СОЗДАНИЕ ДИРЕКТОРИЙ
// ==========================================
const DATA_DIR = path.join(__dirname, 'data');
const UPLOADS_DIR = path.join(__dirname, 'uploads');
const PUBLIC_DIR = path.join(__dirname, 'public');

if (!fs.existsSync(DATA_DIR)) fs.mkdirSync(DATA_DIR, { recursive: true });
if (!fs.existsSync(UPLOADS_DIR)) fs.mkdirSync(UPLOADS_DIR, { recursive: true });
if !fs.existsSync(PUBLIC_DIR)) fs.mkdirSync(PUBLIC_DIR, { recursive: true });

// Подключение к базе данных SQLite
const dbPath = path.join(DATA_DIR, 'tasks.db');
const db = new sqlite3.Database(dbPath);

// ==========================================
// 2. ИНИЦИАЛИЗАЦИЯ ТАБЛИЦ БАЗЫ ДАННЫХ
// ==========================================
db.serialize(() => {
  // Таблица пользователей (с уникальным email и ролями: user, admin, auditor)
  db.run(`
    CREATE TABLE IF NOT EXISTS users (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      username TEXT UNIQUE NOT NULL,
      email TEXT UNIQUE NOT NULL,
      password TEXT NOT NULL,
      role TEXT NOT NULL CHECK(role IN ('user', 'admin', 'auditor')),
      is_blocked INTEGER DEFAULT 0
    )
  `);

  // Таблица для одноразовых токенов сброса пароля
  db.run(`
    CREATE TABLE IF NOT EXISTS password_resets (
      email TEXT NOT NULL,
      token TEXT NOT NULL,
      expires_at DATETIME NOT NULL
    )
  `);

  // Таблица задач (привязана к user_id)
  db.run(`
    CREATE TABLE IF NOT EXISTS tasks (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      user_id INTEGER NOT NULL,
      title TEXT NOT NULL,
      dueDate TEXT NOT NULL,
      status TEXT NOT NULL DEFAULT 'pending' CHECK(status IN ('pending', 'in-progress', 'completed')),
      priority TEXT NOT NULL DEFAULT 'medium' CHECK(priority IN ('low', 'medium', 'high')),
      file TEXT,
      FOREIGN KEY(user_id) REFERENCES users(id) ON DELETE CASCADE
    )
  `);

  // Таблица структурированных логов аудита
  db.run(`
    CREATE TABLE IF NOT EXISTS audit_logs (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
      user_id INTEGER,
      username TEXT,
      action TEXT NOT NULL,
      details TEXT
    )
  `);

  // Таблица активных сессий
  db.run(`
    CREATE TABLE IF NOT EXISTS active_sessions (
      session_id TEXT PRIMARY KEY,
      user_id INTEGER NOT NULL,
      ip TEXT,
      user_agent TEXT,
      created_at DATETIME DEFAULT CURRENT_TIMESTAMP
    )
  `);

  // Инициализация начального главный администратора (admin / admin123)
  db.get("SELECT COUNT(*) as count FROM users", [], (err, row) => {
    if (err) return console.error('Ошибка инициализации пользователей:', err);
    if (row && row.count === 0) {
      const hashAdmin = bcrypt.hashSync('admin123', 10);
      db.run(
        "INSERT INTO users (username, email, password, role) VALUES (?, ?, ?, 'admin')", 
        ['admin', 'admin@example.com', hashAdmin]
      );
      console.log('Создан главный администратор: admin@example.com / admin123');
    }
  });
});

// Настройка Multer для сохранения загружаемых файлов
const storage = multer.diskStorage({
  destination: (req, file, cb) => cb(null, UPLOADS_DIR),
  filename: (req, file, cb) => cb(null, Date.now() + '-' + file.originalname)
});
const upload = multer({ storage });

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Настройка сессий на базе SQLite (кука на 24 часа, без rolling)
app.use(session({
  store: new SQLiteStore({ db: 'sessions.db', dir: DATA_DIR }),
  secret: 'super-secret-key-rbac',
  resave: false,
  saveUninitialized: false,
  rolling: false,
  cookie: { maxAge: 24 * 60 * 60 * 1000 }
}));

app.use(express.static(PUBLIC_DIR));
app.use('/uploads', express.static(UPLOADS_DIR));

// ==========================================
// 3. ЗАЩИТА ОТ БРУТФОРСА (ПОДБОРА ПАРОЛЕЙ)
// ==========================================
// 1. Хранилище неудачных попыток в RAM (IP -> { count, lockUntil })
const failedAttempts = new Map();

// Middleware защиты от перебора
function bruteForceProtection(req, res, next) {
  const ip = req.ip || req.connection.remoteAddress;
  const attempt = failedAttempts.get(ip);

  // Проверяем, находится ли IP в блокировке
  if (attempt && attempt.lockUntil > Date.now()) {
    const remainingSec = Math.ceil((attempt.lockUntil - Date.now()) / 1000);
    return res.status(429).json({ 
      error: `Слишком много неудачных попыток входа. Попробуйте через ${remainingSec} сек.` 
    });
  }
  next();
}

function recordFailedLogin(ip) {
  const now = Date.now();
  const attempt = failedAttempts.get(ip) || { count: 0, lockUntil: 0 };
  attempt.count += 1;
  if (attempt.count >= 5) {
    attempt.lockUntil = now + 15 * 60 * 1000; // Блокировка IP на 15 минут
    attempt.count = 0;
  }
  failedAttempts.set(ip, attempt);
}

function resetFailedLogins(ip) {
  failedAttempts.delete(ip);
}

function deleteFileFromDisk(filename) {
  if (!filename) return;
  const filePath = path.join(UPLOADS_DIR, filename);
  if (fs.existsSync(filePath)) fs.unlinkSync(filePath);
}

// Запись события в логи аудита
function logAudit(userId, username, action, details = null) {
  let formattedDetails = details;
  if (details && typeof details === 'object') {
    // Если передали объект, содержащий ТОЛЬКО taskId — обнуляем его
    if (details.taskId && Object.keys(details).length === 1) {
      formattedDetails = null;
    } else {
      // Иначе создаем копию объекта и вырезаем из нее taskId
      const copy = { ...details };
      delete copy.taskId;
      formattedDetails = Object.keys(copy).length > 0 ? JSON.stringify(copy) : null;
    }
  }
  db.run(
    `INSERT INTO audit_logs (user_id, username, action, details) VALUES (?, ?, ?, ?)`,
    [userId || null, username || 'Guest', action, formattedDetails]
  );
}

// ==========================================
// 4. MIDDLEWARE ПРОВЕРКИ АУТЕНТИФИКАЦИИ И РОЛЕЙ
// ==========================================

// Проверка наличия сессии и синхронизация актуальной роли из БД
function requireAuth(req, res, next) {
  if (!req.session || !req.session.user) {
    return res.status(401).json({ error: 'Необходима авторизация' });
  }

  // Запрашиваем актуальные данные пользователя напрямую из базы данных
  db.get(`SELECT id, username, email, role, is_blocked FROM users WHERE id = ?`, [req.session.user.id], (err, dbUser) => {
    if (err || !dbUser) {
      db.run(`DELETE FROM active_sessions WHERE session_id = ?`, [req.session.id]);
      req.session.destroy();
      return res.status(401).json({ error: 'Сессия недействительна' });
    }

    // Если пользователь заблокирован — завершаем сессию
    if (dbUser.is_blocked) {
      db.run(`DELETE FROM active_sessions WHERE session_id = ?`, [req.session.id]);
      req.session.destroy();
      return res.status(403).json({ error: 'Ваш аккаунт заблокирован' });
    }

    // Синхронизируем роль и данные сессии с актуальной БД
    req.session.user.role = dbUser.role;
    req.session.user.username = dbUser.username;
    req.session.user.email = dbUser.email;

    // Продление временного ключа, если до конца жизни осталось меньше 2 часов
    const now = Date.now();
    const RENEW_THRESHOLD = 2 * 60 * 60 * 1000;
    if (!req.session.expiresAt || (req.session.expiresAt - now < RENEW_THRESHOLD)) {
      req.session.expiresAt = now + 24 * 60 * 60 * 1000;
      req.session.cookie.maxAge = 24 * 60 * 60 * 1000;
    }

    next();
  });
}

// Проверка доступа по ролям (RBAC)
function requireRole(...roles) {
  return (req, res, next) => {
// Проверяем: авторизован ли пользователь и входит ли его роль в список разрешенных
    if (!req.session.user || !roles.includes(req.session.user.role)) {
      // Если роли нет в списке — блокируем запрос
      return res.status(403).json({ error: 'Доступ запрещен: недостаточные права' });
    }
    next(); // Если роль совпадает — пропускаем запрос дальше
  };
}

// ==========================================
// 5. МАРШРУТЫ АУТЕНТИФИКАЦИИ
// ==========================================

// Авторизация по Email и паролю
app.post('/api/auth/login', bruteForceProtection, (req, res) => {
  const { email, password } = req.body;
  if (!email || !password) {
    return res.status(400).json({ error: 'Email и пароль обязательны' });
  }

  // 1. Получаем пользователя из БД по email
  db.get(`SELECT * FROM users WHERE email = ?`, [email.toLowerCase().trim()], (err, user) => {
    if (err) return res.status(500).json({ error: 'Ошибка сервера при входе' });
    if (!user) {
      recordFailedLogin(req.ip);
      logAudit(null, email, 'LOGIN_FAILED', { reason: 'User not found' });
      return res.status(400).json({ error: 'Неверный email или пароль' });
    }
    if (user.is_blocked) {
      return res.status(403).json({ error: 'Аккаунт заблокирован' });
    }

    // 2. Сравниваем чистый пароль из формы с хэшем из БД
    if (bcrypt.compareSync(password, user.password)) {
      resetFailedLogins(req.ip);
      
      const SESSION_TTL = 24 * 60 * 60 * 1000;
      req.session.user = { id: user.id, username: user.username, email: user.email, role: user.role };
      req.session.expiresAt = Date.now() + SESSION_TTL;
      // Запись сессии в БД при успешном входе
      db.run(
        `INSERT OR REPLACE INTO active_sessions (session_id, user_id, ip, user_agent) VALUES (?, ?, ?, ?)`,
        [req.session.id, user.id, req.ip, req.headers['user-agent']]
      );

      logAudit(user.id, user.username, 'LOGIN_SUCCESS');
      res.json({ message: 'Успешный вход', user: req.session.user });
    } else {
      recordFailedLogin(req.ip);
      logAudit(null, email, 'LOGIN_FAILED', { reason: 'Invalid password' });
      res.status(400).json({ error: 'Неверный email или пароль' });
    }
  });
});

// Запрос кода сброса пароля
app.post('/api/auth/forgot-password', (req, res) => {
  const { email } = req.body;
  if (!email) return res.status(400).json({ error: 'Укажите Email' });

  const cleanEmail = email.toLowerCase().trim();
  db.get(`SELECT * FROM users WHERE email = ?`, [cleanEmail], (err, user) => {
    if (err || !user) {
      return res.json({ message: 'Если такой Email существует, инструкция отправлена' });
    }

    // 1. Генерируем 64-символьный случайный крипто-токен
    const token = crypto.randomBytes(16).toString('hex');
    const expiresAt = new Date(Date.now() + 15 * 60 * 1000).toISOString();

    // 2. Сохраняем токен в БД
    db.run(`INSERT INTO password_resets (email, token, expires_at) VALUES (?, ?, ?)`, [cleanEmail, token, expiresAt], (err) => {
      if (err) return res.status(500).json({ error: 'Ошибка при создании токена' });

      console.log(`\n==================================================`);
      console.log(`[EMAIL SERVICE] Запрос на сброс пароля`);
      console.log(`Получатель (Email): ${cleanEmail}`);
      console.log(`Код сброса: ${token}`);
      console.log(`Срок действия: 15 минут`);
      console.log(`==================================================\n`);

      logAudit(user.id, user.username, 'PASSWORD_RESET_REQUESTED');
      res.json({ message: 'Код сброса отправлен! (Проверьте консоль сервера)', token });
    });
  });
});

// Сброс пароля по коду
app.post('/api/auth/reset-password', (req, res) => {
  const { email, token, newPassword } = req.body;
  if (!email || !token || !newPassword) {
    return res.status(400).json({ error: 'Заполните все поля' });
  }

  const cleanEmail = email.toLowerCase().trim();
  // 1. Проверяем токен и срок его действия
  db.get(`SELECT * FROM password_resets WHERE email = ? AND token = ?`, [cleanEmail, token], (err, record) => {
    if (err || !record) return res.status(400).json({ error: 'Неверный код сброса или email' });

    if (new Date(record.expires_at) < new Date()) {
      return res.status(400).json({ error: 'Срок действия кода истек' });
    }

    // 2. Хешируем новый пароль
      const newHash = bcrypt.hashSync(newPassword, 10);

      // 3. Обновляем пароль пользователя
      db.run(`UPDATE users SET password = ? WHERE id = ?`, [newHash, resetRecord.user_id], (err) => {
        if (err) return res.status(500).json({ error: 'Ошибка смены пароля' });

        // 4. Сжигаем использованный токен (Single-use)
        db.run(`DELETE FROM password_resets WHERE token = ?`, [token]);

        // 5. Аннулируем все активные сессии пользователя для безопасности
        db.run(`DELETE FROM active_sessions WHERE user_id = ?`, [resetRecord.user_id]);
      logAudit(null, cleanEmail, 'PASSWORD_RESET_COMPLETED');
      res.json({ message: 'Пароль успешно изменен. Теперь вы можете войти.' });
    });
  });
});

// Выход из системы
app.post('/api/auth/logout', requireAuth, (req, res) => {
  const user = req.session.user;
  db.run(`DELETE FROM active_sessions WHERE session_id = ?`, [req.session.id]);
  logAudit(user.id, user.username, 'LOGOUT');

  req.session.destroy((err) => {
    if (err) return res.status(500).json({ error: 'Ошибка при выходе из системы' });
    res.json({ message: 'Выход выполнен' });
  });
});

// Информация о текущем пользователе
app.get('/api/auth/me', requireAuth, (req, res) => {
  res.json(req.session.user);
});

// ==========================================
// 6. ЛИЧНЫЙ КАБИНЕТ
// ==========================================

app.put('/api/profile/change-password', requireAuth, (req, res) => {
  const { oldPassword, newPassword } = req.body;
  const userId = req.session.user.id;

  if (!oldPassword || !newPassword) return res.status(400).json({ error: 'Заполните оба поля' });

  db.get(`SELECT password FROM users WHERE id = ?`, [userId], (err, user) => {
    if (err || !user) return res.status(500).json({ error: 'Пользователь не найден' });
    if (!bcrypt.compareSync(oldPassword, user.password)) {
      return res.status(400).json({ error: 'Старый пароль указан неверно' });
    }

    const newHash = bcrypt.hashSync(newPassword, 10);
    db.run(`UPDATE users SET password = ? WHERE id = ?`, [newHash, userId], (err) => {
      if (err) return res.status(500).json({ error: 'Не удалось обновить пароль' });
      logAudit(userId, req.session.user.username, 'PASSWORD_CHANGE');
      res.json({ message: 'Пароль успешно изменен' });
    });
  });
});

app.get('/api/profile/sessions', requireAuth, (req, res) => {
  db.all(`SELECT session_id, ip, user_agent, created_at FROM active_sessions WHERE user_id = ?`, [req.session.user.id], (err, rows) => {
    if (err) return res.status(500).json({ error: 'Ошибка получения сессий' });
    res.json(rows.map(r => ({ ...r, isCurrent: r.session_id === req.session.id })));
  });
});

app.delete('/api/profile/sessions/:sid', requireAuth, (req, res) => {
  db.run(`DELETE FROM active_sessions WHERE session_id = ? AND user_id = ?`, [req.params.sid, req.session.user.id], function(err) {
    if (err) return res.status(500).json({ error: 'Ошибка завершения сессии' });
    res.json({ message: 'Сессия завершена' });
  });
});

// ==========================================
// 7. УПРАВЛЕНИЕ ЗАДАЧАМИ (Только для роли 'user')
// ==========================================

app.get('/api/tasks', requireAuth, requireRole('user'), (req, res) => {
  const { status, priority, sortBy, search } = req.query;
  let query = 'SELECT * FROM tasks WHERE user_id = ?';
  const params = [req.session.user.id];

  if (status && status !== 'all') { query += ' AND status = ?'; params.push(status); }
  if (priority && priority !== 'all') { query += ' AND priority = ?'; params.push(priority); }
  if (search) { query += ' AND title LIKE ?'; params.push(`%${search}%`); }

  if (sortBy === 'date_asc') query += ' ORDER BY dueDate ASC, id ASC';
  else if (sortBy === 'date_desc') query += ' ORDER BY dueDate DESC, id DESC';
  else if (sortBy === 'priority_desc') query += ` ORDER BY CASE priority WHEN 'high' THEN 1 WHEN 'medium' THEN 2 WHEN 'low' THEN 3 END ASC`;
  else if (sortBy === 'priority_asc') query += ` ORDER BY CASE priority WHEN 'low' THEN 1 WHEN 'medium' THEN 2 WHEN 'high' THEN 3 END ASC`;
  else query += ' ORDER BY id DESC';

  db.all(query, params, (err, rows) => {
    if (err) return res.status(500).json({ error: 'Ошибка получения задач' });
    res.json(rows);
  });
});

app.post('/api/tasks', requireAuth, requireRole('user'), upload.single('attachment'), (req, res) => {
  const { title, dueDate, priority } = req.body;
  if (!title || !dueDate) return res.status(400).json({ error: 'Название и дата обязательны' });

  const fileName = req.file ? req.file.filename : null;
  const sql = `INSERT INTO tasks (user_id, title, dueDate, status, priority, file) VALUES (?, ?, ?, 'pending', ?, ?)`;

  db.run(sql, [req.session.user.id, title, dueDate, priority || 'medium', fileName], function(err) {
    if (err) return res.status(500).json({ error: 'Ошибка создания задачи' });
    const createdId = this.lastID;
    logAudit(req.session.user.id, req.session.user.username, 'CREATE_TASK', { taskId: createdId });
    db.get(`SELECT * FROM tasks WHERE id = ?`, [createdId], (err, task) => res.status(201).json(task));
  });
});

app.put('/api/tasks/:id', requireAuth, requireRole('user'), upload.single('attachment'), (req, res) => {
  const taskId = req.params.id;
  const { title, dueDate, priority, status, delete_file } = req.body;

  db.get(`SELECT * FROM tasks WHERE id = ? AND user_id = ?`, [taskId, req.session.user.id], (err, task) => {
    if (err || !task) return res.status(404).json({ error: 'Задача не найдена' });

    let currentFile = task.file;
    if (delete_file === 'true' && currentFile) {
      deleteFileFromDisk(currentFile);
      currentFile = null;
    }
    if (req.file) {
      if (currentFile) deleteFileFromDisk(currentFile);
      currentFile = req.file.filename;
    }

    const sql = `UPDATE tasks SET title = ?, dueDate = ?, priority = ?, status = ?, file = ? WHERE id = ? AND user_id = ?`;
    db.run(sql, [title || task.title, dueDate || task.dueDate, priority || task.priority, status || task.status, currentFile, taskId, req.session.user.id], function(err) {
      if (err) return res.status(500).json({ error: 'Ошибка обновления' });
      logAudit(req.session.user.id, req.session.user.username, 'UPDATE_TASK', { taskId });
      db.get(`SELECT * FROM tasks WHERE id = ?`, [taskId], (err, updated) => res.json(updated));
    });
  });
});

app.delete('/api/tasks/:id', requireAuth, requireRole('user'), (req, res) => {
  const taskId = req.params.id;
  db.get(`SELECT file FROM tasks WHERE id = ? AND user_id = ?`, [taskId, req.session.user.id], (err, task) => {
    if (err || !task) return res.status(404).json({ error: 'Задача не найдена' });

    if (task.file) deleteFileFromDisk(task.file);
    db.run(`DELETE FROM tasks WHERE id = ? AND user_id = ?`, [taskId, req.session.user.id], function(err) {
      if (err) return res.status(500).json({ error: 'Ошибка удаления' });
      logAudit(req.session.user.id, req.session.user.username, 'DELETE_TASK', { taskId });
      res.json({ message: 'Удалено' });
    });
  });
});

// ==========================================
// 8. АДМИНИСТРИРОВАНИЕ (Только для роли 'admin')
// ==========================================

// Список пользователей
app.get('/api/admin/users', requireAuth, requireRole('admin'), (req, res) => {
  db.all(`SELECT id, username, email, role, is_blocked FROM users`, [], (err, rows) => {
    if (err) return res.status(500).json({ error: 'Ошибка сервера' });
    res.json(rows);
  });
});

// Создание пользователя администратором (любая роль: user, admin, auditor)
app.post('/api/admin/users', requireAuth, requireRole('admin'), (req, res) => {
  const { email, password, role } = req.body;

  if (!email || !password || !role) {
    return res.status(400).json({ error: 'Заполните все поля: email, password, role' });
  }

  if (!['user', 'admin', 'auditor'].includes(role)) {
    return res.status(400).json({ error: 'Недопустимая роль' });
  }

  const cleanEmail = email.toLowerCase().trim();
  const username = cleanEmail.split('@')[0];
  const hash = bcrypt.hashSync(password, 10);

  const sql = `INSERT INTO users (username, email, password, role) VALUES (?, ?, ?, ?)`;

  db.run(sql, [username, cleanEmail, hash, role], function(err) {
    if (err) {
      if (err.message && err.message.includes('UNIQUE')) {
        return res.status(400).json({ error: 'Пользователь с таким Email уже существует' });
      }
      return res.status(500).json({ error: 'Ошибка при создании пользователя' });
    }

    logAudit(req.session.user.id, req.session.user.username, 'ADMIN_CREATE_USER', { 
      createdUserId: this.lastID, 
      createdEmail: cleanEmail, 
      role 
    });

    res.status(201).json({ message: 'Пользователь успешно создан', userId: this.lastID });
  });
});

// Блокировка/разблокировка любого пользователя (запрещено блокировать только главную учетную запись 'admin')
app.put('/api/admin/users/:id/block', requireAuth, requireRole('admin'), (req, res) => {
  const { is_blocked } = req.body;
  const targetId = req.params.id;

  db.get(`SELECT username FROM users WHERE id = ?`, [targetId], (err, targetUser) => {
    if (err) return res.status(500).json({ error: 'Ошибка сервера при поиске пользователя' });
    if (!targetUser) return res.status(404).json({ error: 'Пользователь не найден' });

    // Запрет блокировки системного администратора 'admin'
    if (targetUser.username === 'admin') {
      return res.status(403).json({ error: 'Запрещено блокировать главного администратора' });
    }

    db.run(`UPDATE users SET is_blocked = ? WHERE id = ?`, [is_blocked ? 1 : 0, targetId], function(err) {
      if (err) return res.status(500).json({ error: 'Ошибка изменения статуса блокировки' });
      
      logAudit(req.session.user.id, req.session.user.username, 'ADMIN_BLOCK_USER', { 
        targetUserId: targetId, 
        targetUsername: targetUser.username,
        is_blocked: !!is_blocked 
      });

      res.json({ message: 'Статус блокировки успешно изменен' });
    });
  });
});

// ==========================================
// 9. АУДИТ И ЛОГИ (Только для роли 'auditor')
// ==========================================

app.get('/api/audit/logs', requireAuth, requireRole('auditor'), (req, res) => {
  db.all(`SELECT * FROM audit_logs ORDER BY timestamp DESC`, [], (err, rows) => {
    if (err) return res.status(500).json({ error: 'Ошибка получения логов' });
    res.json(rows);
  });
});

app.get('/api/audit/logs/export', requireAuth, requireRole('auditor'), (req, res) => {
  db.all(`SELECT * FROM audit_logs ORDER BY timestamp DESC`, [], (err, rows) => {
    if (err) return res.status(500).json({ error: 'Ошибка экспорта' });
    res.setHeader('Content-Type', 'application/json');
    res.setHeader('Content-Disposition', 'attachment; filename=audit_logs.json');
    res.send(JSON.stringify(rows, null, 2));
  });
});

// Перенаправление всех не-API запросов на SPA index.html
app.use((req, res) => {
  res.sendFile(path.join(PUBLIC_DIR, 'index.html'));
});

app.listen(PORT, () => console.log(`Сервер запущен на http://localhost:${PORT}`));