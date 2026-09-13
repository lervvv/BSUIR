const express = require('express');
const multer = require('multer');
const path = require('path');
const fs = require('fs');
const sqlite3 = require('sqlite3').verbose();

const app = express();
const PORT = 3000;

// Подключение к БД SQLite
const db = new sqlite3.Database('tasks.db');

// Создание таблицы задач
db.run(`
  CREATE TABLE IF NOT EXISTS tasks (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    title TEXT NOT NULL,
    dueDate TEXT NOT NULL,
    status TEXT NOT NULL DEFAULT 'pending',
    priority TEXT NOT NULL DEFAULT 'medium',
    file TEXT
  )
`);

// Настройка Multer
const storage = multer.diskStorage({
  destination: (req, file, cb) => cb(null, 'uploads/'),
  filename: (req, file, cb) => cb(null, Date.now() + '-' + file.originalname)
});
const upload = multer({ storage });

app.set('view engine', 'ejs');
app.use(express.urlencoded({ extended: true }));
app.use('/uploads', express.static(path.join(__dirname, 'uploads')));

// Вспомогательная функция для удаления файла с диска
function deleteFileFromDisk(filename) {
  if (!filename) return;
  const filePath = path.join(__dirname, 'uploads', filename);
  if (fs.existsSync(filePath)) {
    fs.unlinkSync(filePath);
  }
}

// 1. Главная страница: чтение, фильтрация и гибкая сортировка
app.get('/', (req, res) => {
  const { filter, sort } = req.query;
  let query = 'SELECT * FROM tasks';
  const params = [];

  // 1. Фильтрация по статусу
  if (filter && filter !== 'all') {
    query += ' WHERE status = ?';
    params.push(filter);
  }

  // 2. Сортировка (по приоритету, сначала новые, сначала старые)
  if (sort === 'priority') {
    query += ` ORDER BY CASE priority 
                WHEN 'high' THEN 1 
                WHEN 'medium' THEN 2 
                WHEN 'low' THEN 3 
                ELSE 4 END ASC, id DESC`;
  } else if (sort === 'oldest') {
    query += ' ORDER BY id ASC'; // Сначала старые
  } else {
    query += ' ORDER BY id DESC'; // По умолчанию: сначала новые (newest)
  }

  db.all(query, params, (err, rows) => {
    if (err) return res.status(500).send('Ошибка БД');
    res.render('index', { 
      tasks: rows, 
      currentFilter: filter || 'all',
      currentSort: sort || 'newest'
    });
  });
});

// 2. Создание задачи
app.post('/tasks', upload.single('attachment'), (req, res) => {
  const { title, dueDate, priority } = req.body;
  const fileName = req.file ? req.file.filename : null;

  const sql = `INSERT INTO tasks (title, dueDate, status, priority, file) VALUES (?, ?, 'pending', ?, ?)`;
  db.run(sql, [title, dueDate, priority || 'medium', fileName], () => {
    res.redirect('/');
  });
});

// 3. Обновление статуса
app.post('/tasks/:id/status', (req, res) => {
  const sql = `UPDATE tasks SET status = ? WHERE id = ?`;
  db.run(sql, [req.body.status, req.params.id], () => {
    res.redirect('/');
  });
});

// 4. Редактирование задачи (название, дата, приоритет, управление файлом)
app.post('/tasks/:id/edit', upload.single('attachment'), (req, res) => {
  const taskId = req.params.id;
  const { title, dueDate, priority, delete_file } = req.body;

  db.get(`SELECT file FROM tasks WHERE id = ?`, [taskId], (err, task) => {
    if (err || !task) return res.redirect('/');

    let currentFile = task.file;

    // Если поставлена галочка "Удалить файл"
    if (delete_file === 'true' && currentFile) {
      deleteFileFromDisk(currentFile);
      currentFile = null;
    }

    // Если загружен новый файл (заменяет старый)
    if (req.file) {
      if (currentFile) deleteFileFromDisk(currentFile);
      currentFile = req.file.filename;
    }

    const sql = `UPDATE tasks SET title = ?, dueDate = ?, priority = ?, file = ? WHERE id = ?`;
    db.run(sql, [title, dueDate, priority, currentFile, taskId], () => {
      res.redirect('/');
    });
  });
});

// 5. Удаление задачи полностью
app.post('/tasks/:id/delete', (req, res) => {
  const taskId = req.params.id;

  db.get(`SELECT file FROM tasks WHERE id = ?`, [taskId], (err, task) => {
    if (task && task.file) {
      deleteFileFromDisk(task.file);
    }
    db.run(`DELETE FROM tasks WHERE id = ?`, [taskId], () => {
      res.redirect('/');
    });
  });
});

app.listen(PORT, () => {
  console.log(`Сервер с БД работает на http://localhost:${PORT}`);
});