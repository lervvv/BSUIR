let currentUser = null;
let currentTasks = [];

document.addEventListener('DOMContentLoaded', () => {
  checkAuthAndInit();

  const loginForm = document.getElementById('loginForm');
  if (loginForm) loginForm.addEventListener('submit', login);

  const forgotForm = document.getElementById('forgotForm');
  if (forgotForm) forgotForm.addEventListener('submit', sendForgotRequest);

  const resetForm = document.getElementById('resetForm');
  if (resetForm) resetForm.addEventListener('submit', resetPassword);

  const createForm = document.getElementById('createForm');
  if (createForm) createForm.addEventListener('submit', createTask);

  const editForm = document.getElementById('editForm');
  if (editForm) editForm.addEventListener('submit', updateTask);

  const passwordForm = document.getElementById('passwordForm');
  if (passwordForm) passwordForm.addEventListener('submit', changePassword);

  const createUserForm = document.getElementById('createUserForm');
  if (createUserForm) createUserForm.addEventListener('submit', createUser);

  ['filterStatus', 'filterPriority', 'sortBy'].forEach(id => {
    const el = document.getElementById(id);
    if (el) el.addEventListener('change', loadTasks);
  });

  const searchInput = document.getElementById('searchInput');
  if (searchInput) searchInput.addEventListener('input', loadTasks);
});

// Полная очистка полей ввода и чекбоксов показа паролей
function clearAllForms() {
  const forms = document.querySelectorAll('form');
  forms.forEach(form => form.reset());

  const passwordInputs = document.querySelectorAll('input');
  passwordInputs.forEach(input => {
    if (input.id && (input.id.toLowerCase().includes('password') || input.id.toLowerCase().includes('pwd'))) {
      input.type = 'password';
      input.value = '';
    }
  });

  const pwdCheckboxes = document.querySelectorAll('.show-pwd-checkbox');
  pwdCheckboxes.forEach(cb => { cb.checked = false; });
}

// Переключение видимости пароля по галочке чекбокса
function togglePassword(inputId, checkbox) {
  const input = document.getElementById(inputId);
  if (!input) return;
  input.type = checkbox.checked ? 'text' : 'password';
}

function toggleForgotForm(show) {
  clearAllForms();
  const forgotForm = document.getElementById('forgotForm');
  if (show) forgotForm.classList.remove('hidden');
  else forgotForm.classList.add('hidden');
}

// Проверка сессии при запуске приложения
async function checkAuthAndInit() {
  try {
    const res = await fetch('/api/auth/me');
    if (!res.ok) {
      showAuthSection();
      return;
    }
    
    currentUser = await res.json();
    showAppSection();
  } catch (err) {
    showAuthSection();
  }
}

function showAuthSection() {
  clearAllForms();
  document.getElementById('authSection').classList.remove('hidden');
  document.getElementById('appSection').classList.add('hidden');
  currentUser = null;
}

// Отрисовка интерфейса и навигации в зависимости от роли пользователя (RBAC)
function showAppSection() {
  clearAllForms();
  document.getElementById('authSection').classList.add('hidden');
  document.getElementById('appSection').classList.remove('hidden');

  const navTasks = document.getElementById('navTasks'); // Кнопка "Задачи"
  const navAdmin = document.getElementById('navAdmin'); // Кнопка "Управление пользователями"
  const navAudit = document.getElementById('navAudit'); // Кнопка "Логи аудита"

  // 1. Если вошел АДМИНИСТРАТОР:
  if (currentUser.role === 'admin') {
    navTasks.classList.add('hidden');    // Скрываем задачи
    navAdmin.classList.remove('hidden'); // Показываем управление пользователями
    navAudit.classList.add('hidden');    // Скрываем логи
    showTab('adminTab');                 // Открываем вкладку админа по умолчанию

  // 2. Если вошел АУДИТОР:
  } else if (currentUser.role === 'auditor') {
    navTasks.classList.add('hidden');    // Скрываем задачи
    navAdmin.classList.add('hidden');    // Скрываем управление пользователями
    navAudit.classList.remove('hidden'); // Показываем логи аудита
    showTab('auditTab');                 // Открываем вкладку аудитора по умолчанию

  // 3. Если вошел ОБЫЧНЫЙ ПОЛЬЗОВАТЕЛЬ (user):
  } else {
    navTasks.classList.remove('hidden'); // Показываем задачи
    navAdmin.classList.add('hidden');    // Скрываем управление пользователями
    navAudit.classList.add('hidden');    // Скрываем логи
    showTab('tasksTab');                 // Открываем задачи по умолчанию
  }
}

// Выполнение входа в систему
async function login(e) {
  e.preventDefault();
  const email = document.getElementById('loginEmail').value.trim();
  const password = document.getElementById('loginPassword').value;

  try {
    const res = await fetch('/api/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });

    const data = await res.json();
    if (!res.ok) throw new Error(data.error || 'Ошибка входа');

    clearAllForms();
    showToast('Успешный вход!');
    checkAuthAndInit();
  } catch (err) {
    showToast(err.message);
  }
}

async function sendForgotRequest(e) {
  e.preventDefault();
  const email = document.getElementById('forgotEmail').value.trim();

  try {
    const res = await fetch('/api/auth/forgot-password', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email })
    });
    const data = await res.json();
    if (!res.ok) throw new Error(data.error || 'Ошибка');

    showToast(data.message);
    document.getElementById('resetEmail').value = email;
    if (data.token) document.getElementById('resetToken').value = data.token;
    
    document.getElementById('resetNewPassword').value = '';
    
    document.getElementById('forgotForm').classList.add('hidden');
    document.getElementById('resetForm').classList.remove('hidden');
  } catch (err) {
    showToast(err.message);
  }
}

async function resetPassword(e) {
  e.preventDefault();
  const email = document.getElementById('resetEmail').value.trim();
  const token = document.getElementById('resetToken').value.trim();
  const newPassword = document.getElementById('resetNewPassword').value;

  try {
    const res = await fetch('/api/auth/reset-password', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, token, newPassword })
    });
    const data = await res.json();
    if (!res.ok) throw new Error(data.error || 'Ошибка');

    showToast(data.message);
    clearAllForms();
    document.getElementById('resetForm').classList.add('hidden');
  } catch (err) {
    showToast(err.message);
  }
}

async function logout() {
  try {
    await fetch('/api/auth/logout', { method: 'POST' });
    showToast('Выход выполнен');
  } catch (err) {
  } finally {
    showAuthSection();
  }
}

// Переключение вкладок
function showTab(tabId) {
  clearAllForms();

  const tabs = document.querySelectorAll('.tab-content');
  tabs.forEach(tab => tab.classList.add('hidden'));

  const activeTab = document.getElementById(tabId);
  if (activeTab) activeTab.classList.remove('hidden');

  const navButtons = document.querySelectorAll('.nav .btn:not(.btn-danger)');
  navButtons.forEach(btn => btn.classList.remove('active'));

  const btnMap = {
    'tasksTab': 'navTasks',
    'profileTab': 'navProfile',
    'adminTab': 'navAdmin',
    'auditTab': 'navAudit'
  };
  
  const activeBtnId = btnMap[tabId];
  if (activeBtnId) {
    const activeBtn = document.getElementById(activeBtnId);
    if (activeBtn) activeBtn.classList.add('active');
  }

  if (tabId === 'tasksTab') loadTasks();
  if (tabId === 'profileTab') {
    renderProfileInfo();
    loadSessions();
  }
  if (tabId === 'adminTab') loadUsers();
  if (tabId === 'auditTab') loadAuditLogs();
}

function renderProfileInfo() {
  if (!currentUser) return;
  const roleMap = { admin: 'Администратор', auditor: 'Аудитор', user: 'Пользователь' };
  document.getElementById('profileEmail').innerText = currentUser.email || '—';
  document.getElementById('profileRole').innerText = roleMap[currentUser.role] || currentUser.role;
}

function showToast(message) {
  const toast = document.getElementById('toast');
  if (!toast) return;
  toast.innerText = message;
  toast.className = 'show';
  setTimeout(() => { toast.className = toast.className.replace('show', ''); }, 3000);
}

// УПРАВЛЕНИЕ ЗАДАЧАМИ
async function loadTasks() {
  const status = document.getElementById('filterStatus')?.value || 'all';
  const priority = document.getElementById('filterPriority')?.value || 'all';
  const sortBy = document.getElementById('sortBy')?.value || 'newest';
  const search = document.getElementById('searchInput')?.value || '';

  const queryParams = new URLSearchParams({ status, priority, sortBy, search });

  try {
    const res = await fetch(`/api/tasks?${queryParams}`);
    if (res.status === 401 || res.status === 403) { showAuthSection(); return; }
    if (!res.ok) throw new Error('Ошибка загрузки задач');
    
    currentTasks = await res.json();
    renderTasks(currentTasks);
  } catch (err) {
    showToast(err.message);
  }
}

function renderTasks(tasks) {
  const container = document.getElementById('taskList');
  if (!container) return;
  container.innerHTML = '';

  if (!tasks || tasks.length === 0) {
    container.innerHTML = '<p style="text-align: center; color: #888;">Задач не найдено</p>';
    return;
  }

  const priorityMap = { high: 'Высокий', medium: 'Средний', low: 'Низкий' };
  const statusMap = { pending: 'Ожидает', 'in-progress': 'В процессе', completed: 'Завершено' };

  tasks.forEach((task, index) => {
    const card = document.createElement('div');
    card.className = 'task-item';
    
    card.innerHTML = `
      <div class="task-header">
        <h3 class="task-title">${escapeHtml(task.title)}</h3>
        <span class="badge">${statusMap[task.status] || task.status}</span>
      </div>
      <div class="task-meta">
        <span>📅 До: <strong>${escapeHtml(task.dueDate)}</strong></span>
        <span>⚡ Приоритет: <strong>${priorityMap[task.priority] || task.priority}</strong></span>
      </div>
      ${task.file ? `<p><a href="/uploads/${escapeHtml(task.file)}" target="_blank">📎 Прикрепленный файл</a></p>` : ''}
      <div class="task-actions">
        <button class="btn btn-secondary" onclick="openEditModalByIndex(${index})">Изменить</button>
        <button class="btn btn-danger" onclick="deleteTask(${task.id})">Удалить</button>
      </div>
    `;
    container.appendChild(card);
  });
}

async function createTask(e) {
  e.preventDefault();
  const formData = new FormData();
  formData.append('title', document.getElementById('createTitle').value);
  formData.append('dueDate', document.getElementById('createDueDate').value);
  formData.append('priority', document.getElementById('createPriority').value);
  
  const fileInput = document.getElementById('createFile');
  if (fileInput && fileInput.files[0]) {
    formData.append('attachment', fileInput.files[0]);
  }

  try {
    const res = await fetch('/api/tasks', { method: 'POST', body: formData });
    const data = await res.json();

    if (!res.ok) throw new Error(data.error || 'Ошибка создания задачи');
    
    document.getElementById('createForm').reset();
    showToast('Задача успешно создана!');
    loadTasks();
  } catch (err) {
    showToast(err.message);
  }
}

function openEditModalByIndex(index) {
  const task = currentTasks[index];
  if (!task) return;

  document.getElementById('editId').value = task.id;
  document.getElementById('editTitle').value = task.title;
  document.getElementById('editDueDate').value = task.dueDate;
  document.getElementById('editPriority').value = task.priority;
  document.getElementById('editStatus').value = task.status;
  
  const deleteFileContainer = document.getElementById('deleteFileContainer');
  const editDeleteFile = document.getElementById('editDeleteFile');
  
  if (deleteFileContainer) {
    if (task.file) {
      deleteFileContainer.style.display = 'block';
      if (editDeleteFile) editDeleteFile.checked = false;
    } else {
      deleteFileContainer.style.display = 'none';
    }
  }

  document.getElementById('editModal').classList.remove('hidden');
}

function closeModal() {
  document.getElementById('editModal').classList.add('hidden');
}

async function updateTask(e) {
  e.preventDefault();
  const id = document.getElementById('editId').value;
  const formData = new FormData();
  
  formData.append('title', document.getElementById('editTitle').value);
  formData.append('dueDate', document.getElementById('editDueDate').value);
  formData.append('priority', document.getElementById('editPriority').value);
  formData.append('status', document.getElementById('editStatus').value);
  
  const fileInput = document.getElementById('editFile');
  if (fileInput && fileInput.files[0]) {
    formData.append('attachment', fileInput.files[0]);
  }
  
  const deleteFileCheckbox = document.getElementById('editDeleteFile');
  if (deleteFileCheckbox && deleteFileCheckbox.checked) {
    formData.append('delete_file', 'true');
  }

  try {
    const res = await fetch(`/api/tasks/${id}`, { method: 'PUT', body: formData });
    const data = await res.json();

    if (!res.ok) throw new Error(data.error || 'Ошибка обновления задачи');

    closeModal();
    showToast('Задача успешно обновлена!');
    loadTasks();
  } catch (err) {
    showToast(err.message);
  }
}

async function deleteTask(id) {
  if (!confirm('Удалить эту задачу?')) return;

  try {
    const res = await fetch(`/api/tasks/${id}`, { method: 'DELETE' });
    const data = await res.json();

    if (!res.ok) throw new Error(data.error || 'Ошибка удаления');

    showToast('Задача удалена');
    loadTasks();
  } catch (err) {
    showToast(err.message);
  }
}

// ЛИЧНЫЙ КАБИНЕТ
async function changePassword(e) {
  e.preventDefault();
  const oldPassword = document.getElementById('oldPassword').value;
  const newPassword = document.getElementById('newPassword').value;

  try {
    const res = await fetch('/api/profile/change-password', {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ oldPassword, newPassword })
    });

    const data = await res.json();
    if (!res.ok) throw new Error(data.error || 'Ошибка смены пароля');

    clearAllForms();
    showToast('Пароль успешно изменен!');
  } catch (err) {
    showToast(err.message);
  }
}

function formatIp(ip) {
  if (!ip) return '—';
  return ip.replace(/^.*:/, '');
}

function parseUserAgent(ua) {
  if (!ua) return 'Неизвестно';
  let os = 'Неизвестная ОС';
  let browser = 'Браузер';

  if (ua.includes('Windows')) os = 'Windows';
  else if (ua.includes('Macintosh') || ua.includes('Mac OS')) os = 'macOS';
  else if (ua.includes('Linux')) os = 'Linux';
  else if (ua.includes('Android')) os = 'Android';
  else if (ua.includes('iPhone') || ua.includes('iPad')) os = 'iOS';

  if (ua.includes('Edg/')) browser = 'Edge';
  else if (ua.includes('Chrome/') && !ua.includes('Edg/')) browser = 'Chrome';
  else if (ua.includes('Safari/') && !ua.includes('Chrome/')) browser = 'Safari';
  else if (ua.includes('Firefox/')) browser = 'Firefox';

  return `${browser} (${os})`;
}

async function loadSessions() {
  try {
    const res = await fetch('/api/profile/sessions');
    if (!res.ok) return;

    const sessions = await res.json();
    const container = document.getElementById('sessionsList');
    if (!container) return;

    if (sessions.length === 0) {
      container.innerHTML = '<p>Нет активных сессий</p>';
      return;
    }

    let html = '<table><tr><th>IP</th><th>Браузер / ОС</th><th>Создана</th><th>Действие</th></tr>';
    sessions.forEach(s => {
      html += `<tr>
        <td>${escapeHtml(formatIp(s.ip))}</td>
        <td>${escapeHtml(parseUserAgent(s.user_agent))}</td>
        <td>${escapeHtml(s.created_at)}</td>
        <td>${s.isCurrent ? '<strong>(Текущая)</strong>' : `<button class="btn btn-danger" onclick="terminateSession('${s.session_id}')">Завершить</button>`}</td>
      </tr>`;
    });
    html += '</table>';
    container.innerHTML = html;
  } catch (err) {
    showToast('Ошибка загрузки сессий');
  }
}

async function terminateSession(sid) {
  try {
    const res = await fetch(`/api/profile/sessions/${sid}`, { method: 'DELETE' });
    if (!res.ok) throw new Error('Ошибка завершения сессии');
    showToast('Сессия завершена');
    loadSessions();
  } catch (err) {
    showToast(err.message);
  }
}

// ==========================================
// АДМИНИСТРИРОВАНИЕ И БЛОКИРОВКА ПОЛЬЗОВАТЕЛЕЙ
// ==========================================

async function createUser(e) {
  e.preventDefault();
  const email = document.getElementById('newUserEmail').value.trim();
  const password = document.getElementById('newUserPassword').value;
  const role = document.getElementById('newUserRole').value;

  try {
    const res = await fetch('/api/admin/users', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password, role })
    });

    const data = await res.json();
    if (!res.ok) throw new Error(data.error || 'Ошибка создания пользователя');

    clearAllForms();
    showToast('Пользователь успешно создан!');
    loadUsers();
  } catch (err) {
    showToast(err.message);
  }
}

// Загрузка списка пользователей
async function loadUsers() {
  try {
    const res = await fetch('/api/admin/users');
    if (!res.ok) return;

    const users = await res.json();
    const container = document.getElementById('usersList');
    if (!container) return;

    const roleMap = { admin: 'Администратор', auditor: 'Аудитор', user: 'Пользователь' };

    let html = '<table><tr><th>ID</th><th>Email</th><th>Роль</th><th>Статус</th><th>Действия</th></tr>';
    users.forEach(u => {
      const isHardcodedAdmin = u.username === 'admin';
      html += `<tr>
        <td>${u.id}</td>
        <td>${escapeHtml(u.email || u.username)}</td>
        <td>${roleMap[u.role] || escapeHtml(u.role)}</td>
        <td>${u.is_blocked ? '<span style="color:red;">Заблокирован</span>' : '<span style="color:green;">Активен</span>'}</td>
        <td>
          ${isHardcodedAdmin 
            ? `<button class="btn btn-secondary" disabled style="opacity: 0.5; cursor: not-allowed;" title="Нельзя заблокировать главного администратора">Заблокировать</button>`
            : `<button class="btn ${u.is_blocked ? '' : 'btn-danger'}" onclick="toggleBlock(${u.id}, ${!u.is_blocked}, '${escapeHtml(u.username)}')">
                ${u.is_blocked ? 'Разблокировать' : 'Заблокировать'}
              </button>`
          }
        </td>
      </tr>`;
    });
    html += '</table>';
    container.innerHTML = html;
  } catch (err) {
    showToast('Ошибка загрузки пользователей');
  }
}

// Переключение блокировки любого пользователя администратором
async function toggleBlock(userId, is_blocked, username) {
  if (username === 'admin') {
    showToast('Нельзя заблокировать главного администратора');
    return;
  }

  try {
    const res = await fetch(`/api/admin/users/${userId}/block`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ is_blocked })
    });
    const data = await res.json();
    if (!res.ok) throw new Error(data.error || 'Ошибка изменения статуса');
    
    showToast(data.message || 'Статус блокировки изменен');
    loadUsers();
  } catch (err) {
    showToast(err.message);
  }
}

// ЛОГИ АУДИТА
async function loadAuditLogs() {
  try {
    const res = await fetch('/api/audit/logs');
    if (!res.ok) return;

    const logs = await res.json();
    const container = document.getElementById('auditLogsList');
    if (!container) return;

    let html = '<table><tr><th>Время</th><th>Пользователь</th><th>Действие</th><th>Детали</th></tr>';
    logs.forEach(l => {
      html += `<tr>
        <td>${escapeHtml(l.timestamp)}</td>
        <td>${escapeHtml(l.username || '—')}</td>
        <td><strong>${escapeHtml(l.action)}</strong></td>
        <td>${escapeHtml(l.details || '—')}</td>
      </tr>`;
    });
    html += '</table>';
    container.innerHTML = html;
  } catch (err) {
    showToast('Ошибка загрузки логов');
  }
}

function escapeHtml(text) {
  if (!text) return '';
  return String(text).replace(/[&<>"']/g, m => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#039;' }[m]));
}