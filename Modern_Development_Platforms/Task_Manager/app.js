const express = require('express');
const multer = require('multer');
const path = require('path');

const app = express();
const PORT = 3000;

// 1. Настройка хранилища для загружаемых файлов (Multer)
const storage = multer.diskStorage({
  destination: (req, file, cb) => {
    cb(null, 'uploads/'); // Файлы будут сохраняться в папку uploads
  },
  filename: (req, file, cb) => {
    // Добавляем timestamp к имени файла, чтобы избежать дубликатов
    cb(null, Date.now() + '-' + file.originalname);
  }
});
const upload = multer({ storage });

// 2. Настройка шаблонизатора EJS и парсинга данных форм
app.set('view engine', 'ejs');
app.use(express.urlencoded({ extended: true })); // Для чтения данных из обычных HTML-форм
app.use('/uploads', express.static(path.join(__dirname, 'uploads'))); // Статический доступ к загруженным файлам

// 3. Массив в памяти для хранения списка задач
let tasks = [
  {
    id: 1,
    title: 'Сделать Лабораторную №1',
    dueDate: '2026-09-15',
    status: 'in-progress',
    file: null
  }
];

// 4. Главная страница (GET /) — вывод списка задач с поддержкой фильтрации
app.get('/', (req, res) => {
  const { filter } = req.query;
  let filteredTasks = tasks;

  if (filter && filter !== 'all') {
    filteredTasks = tasks.filter(task => task.status === filter);
  }

  // Отдаем клиенту готовую HTML-разметку (Server-Side Rendering)
  res.render('index', { tasks: filteredTasks, currentFilter: filter || 'all' });
});

// 5. Создание задачи (POST /tasks) — прием данных формы + загрузка файла
app.post('/tasks', upload.single('attachment'), (req, res) => {
  const { title, dueDate } = req.body;
  
  const newTask = {
    id: Date.now(),
    title,
    dueDate,
    status: 'pending',
    file: req.file ? req.file.filename : null // Если файл прикреплен, сохраняем его имя
  };

  tasks.push(newTask);
  res.redirect('/'); // Перенаправляем пользователя обратно на главную
});

// 6. Изменение статуса задачи (POST /tasks/:id/status)
app.post('/tasks/:id/status', (req, res) => {
  const taskId = parseInt(req.params.id);
  const { status } = req.body;

  const task = tasks.find(t => t.id === taskId);
  if (task) {
    task.status = status;
  }

  res.redirect('/');
});

// Запуск сервера
app.listen(PORT, () => {
  console.log(`Сервер успешно запущен! Откройте браузер: http://localhost:${PORT}`);
});