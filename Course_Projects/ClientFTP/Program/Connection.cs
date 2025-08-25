using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientFTP
{
    public partial class Connection : Form
    {
        private FtpServerInfo serverInfo;
        private FileInformation fileInfo;
        private FileInformation fileToMove;
        private TreeNode selectedNode; 
        private bool isMoveMode;
        public Connection()
        {
            InitializeComponent();
            btnDelete.Enabled = false;
            btnRename.Enabled = false;
            btnMove.Enabled = false;
            LoadLocalInformation();
            trvLocal.BeforeExpand += trvLocal_BeforeExpand;

            trvLocal.AllowDrop = true;
            trvServer.AllowDrop = true;

            trvLocal.ItemDrag += trvLocal_ItemDrag;
            trvLocal.DragEnter += trvLocal_DragEnter;
            trvLocal.DragDrop += trvLocal_DragDrop;

            trvServer.ItemDrag += trvServer_ItemDrag;
            trvServer.DragEnter += trvServer_DragEnter;
            trvServer.DragDrop += trvServer_DragDrop;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!IPAddress.TryParse(txtIp.Text, out _) && Uri.CheckHostName(txtIp.Text) == UriHostNameType.Unknown)
            {
                MessageBox.Show("Введите корректный IP-адрес или доменное имя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtPort.Text != "")
            {
                if (!int.TryParse(txtPort.Text, out int port) || port < 1 || port > 65535)
                {
                    MessageBox.Show("Введите корректный порт!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            try
            {
                string ip = txtIp.Text;
                string port = txtPort.Text;
                string userName = txtLogin.Text;
                string password = txtPassword.Text;

                //формирование FTP-URL
                string ftpUrl = $"ftp://{ip}:{port}/";
                //создание запроса
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                //запрос на получение списка файлов
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                //указание логина и пароля, если требуется авторизация
                request.Credentials = new NetworkCredential(userName, password);
                //сервер сам открывает порт
                request.UsePassive = true;
                //передача файлов в бинарном виде
                request.UseBinary = true;
                //после выполнения команды соединение закрывается
                request.KeepAlive = false;

                //получение ответа от сервера
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    MessageBox.Show("Успешное подключение!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        
                    serverInfo = new FtpServerInfo
                    {
                        Ip = txtIp.Text,
                        Port = txtPort.Text,
                        UserName = string.IsNullOrEmpty(txtLogin.Text) ? "anonymous" : txtLogin.Text,
                        Password = txtPassword.Text
                    };
                    LoadFtpInformation();
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadFtpInformation()
        {
            if (serverInfo != null)
            {
                trvServer.Nodes.Clear();
                var rootNode = new TreeNode(serverInfo.Ip);
                trvServer.Nodes.Add(rootNode);
                await LoadFilesRecursive("/", rootNode);
            }
        }

        private async Task LoadFilesRecursive(string currentPath, TreeNode parentNode)
        {
            string ftpUrl = $"ftp://{serverInfo.Ip}:{serverInfo.Port}{currentPath}";
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                //получение списка файлов с подробной информацией
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                using (var response = (FtpWebResponse)await request.GetResponseAsync())
                //извлечение потока Stream со списком файлов и каталогов
                using (var stream = response.GetResponseStream())
                //оборачивает Stream в StreamReader, чтобы читать построчно
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = await reader.ReadLineAsync();
                        //извлечение имени папки или файла
                        string name = ExtractName(line);
                        bool isDirectory = IsDirectory(line);

                        var node = new TreeNode(name)
                        {
                            Tag = isDirectory
                        };
                        parentNode.Nodes.Add(node);

                        if (isDirectory)
                        {
                            string nextPath = currentPath.EndsWith("/")
                            ? currentPath + name
                            : currentPath + "/" + name;
                            //рекурсивный вызов для папки
                            await LoadFilesRecursive(nextPath + "/", node);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении данных сервера: {ex.Message}", "Ошибка");
            }
        }

        //для извлечения имени файла или папки
        private string ExtractName(string line)
        {
            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return parts[parts.Length - 1]; //последняя часть — имя файла или папки
        }

        private bool IsDirectory(string line)
        {
            //если строка начинается с d - папка
            return line.StartsWith("d");
        }

        private void LoadLocalInformation()
        {
            trvLocal.Nodes.Clear();

            TreeNode rootNode = new TreeNode("Мой компьютер");
            trvLocal.Nodes.Add(rootNode);
            //получение списка всех доступных дисков
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                TreeNode driveNode = new TreeNode(drive.Name)
                {
                    //сохранение информации о папке в тег
                    Tag = drive.RootDirectory
                };
                //добавление заглушки для раскрываемости
                driveNode.Nodes.Add("*dummy*");

                rootNode.Nodes.Add(driveNode);
            }
        }

        private void trvLocal_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "*dummy*")
            {
                e.Node.Nodes.Clear();
                //преобразование тега в DirectoryInfo, чтобы получить доступ к методам GetDirectories и GetFiles
                if (e.Node.Tag is DirectoryInfo dir)
                {
                    try
                    {
                        foreach (DirectoryInfo subDir in dir.GetDirectories())
                        {
                            TreeNode dirNode = new TreeNode(subDir.Name)
                            {
                                //сохранение инф о текущей папке
                                Tag = subDir,
                            };
                            dirNode.Nodes.Add("*dummy*");
                            e.Node.Nodes.Add(dirNode);
                        }

                        foreach (FileInfo file in dir.GetFiles())
                        {
                            TreeNode fileNode = new TreeNode(file.Name)
                            {
                                Tag = file,
                            };
                            e.Node.Nodes.Add(fileNode);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        e.Node.Nodes.Add("Нет доступа");
                    }
                }
            }
        }

        //начало перетаскивания с локального комп
        private void trvLocal_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is TreeNode node && node.Tag is FileInfo fileInfo)
            {
                //перетаскивает локальный файл с возможностью копирования
                DoDragDrop(new LocalFileDragData(fileInfo.FullName), DragDropEffects.Copy);
            }
        }

        //обработка перетаскивания на локальный комп
        private void trvLocal_DragEnter(object sender, DragEventArgs e)
        {
            //если перетаскиваемые данные - объект типа RemoteFileDragData
            if (e.Data.GetDataPresent(typeof(RemoteFileDragData)))
            {
                //устанавливается эффект копирования
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //обработка копирования на локальный комп
        private async void trvLocal_DragDrop(object sender, DragEventArgs e)
        {
            //если перетаскиваемые данные - объект типа RemoteFileDragData
            if (e.Data.GetDataPresent(typeof(RemoteFileDragData)))
            {
                var remoteData = (RemoteFileDragData)e.Data.GetData(typeof(RemoteFileDragData));
                string remotePath = remoteData.RemotePath;

                //узел, на который отпускают файл с сервера по положению мыши
                TreeNode targetNode = trvLocal.GetNodeAt(trvLocal.PointToClient(new Point(e.X, e.Y)));
                string localPath = GetLocalTargetPath(targetNode);

                if (string.IsNullOrEmpty(localPath))
                {
                    MessageBox.Show("Не удалось определить путь для сохранения.", "Ошибка");
                    return;
                }

                await DownloadFileFromFtp(remotePath, localPath);

                LoadLocalInformation();
            }
        }

        //начало перетаскивания с сервера
        private void trvServer_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is TreeNode node && node.Tag is bool isDirectory && !isDirectory)
            {
                string remotePath = GetFullRemotePath(node);
                //перетаскивает файл с возможностью копирования
                DoDragDrop(new RemoteFileDragData(remotePath), DragDropEffects.Copy);
            }
        }

        //перетаскивание на сервер
        private void trvServer_DragEnter(object sender, DragEventArgs e)
        {
            //копирование для локального файла
            if (e.Data.GetDataPresent(typeof(LocalFileDragData)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //копирование файла на сервер
        private async void trvServer_DragDrop(object sender, DragEventArgs e)
        {
            //данные о перетаскиваемом локальном файле
            if (e.Data.GetDataPresent(typeof(LocalFileDragData)))
            {
                var localData = (LocalFileDragData)e.Data.GetData(typeof(LocalFileDragData));
                string localPath = localData.LocalPath;

                //узел, на который отпускают файл с сервера по положению мыши
                TreeNode targetNode = trvServer.GetNodeAt(trvServer.PointToClient(new Point(e.X, e.Y)));
                string remotePath;

                //если путь пустой - в корень сохранение
                if (targetNode == null) 
                    remotePath =  "/";
                //если выбрана папка - в папку
                else if (targetNode.Tag is bool isDir && isDir)
                    remotePath = GetFullRemotePath(targetNode);
                //если выбран файл - в родительскую папку
                else if (targetNode.Parent != null)
                    remotePath = GetFullRemotePath(targetNode.Parent);
                else
                    remotePath = "/";

                if (string.IsNullOrEmpty(remotePath))
                {
                    MessageBox.Show("Не удалось определить путь на сервере.", "Ошибка");
                    return;
                }

                await UploadFileToFtp(localPath, remotePath);

                LoadFtpInformation();
            }
        }

        [Serializable] //обязательное условие для объектов, передающихся через DoDragDrop
        public class LocalFileDragData
        {
            public string LocalPath { get; }

            public LocalFileDragData(string path)
            {
                LocalPath = path;
            }
        }

        [Serializable]
        public class RemoteFileDragData
        {
            public string RemotePath { get; }

            public RemoteFileDragData(string path)
            {
                RemotePath = path;
            }
        }

        //полный путь на сервере
        private string GetFullRemotePath(TreeNode node)
        {
            if (node == null) return string.Empty;

            string path = node.Text;
            TreeNode parent = node.Parent;

            while (parent != null && parent != trvServer.Nodes[0])
            {
                path = parent.Text + "/" + path;
                parent = parent.Parent;
            }

            return path;
        }

        //локальный путь 
        private string GetLocalTargetPath(TreeNode node)
        {
            //если узел не найден - рабочий стол
            if (node == null) return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (node.Tag is DirectoryInfo dirInfo)
            {
                return dirInfo.FullName;
            }
            //если узел диск - путь к его корню
            else if (node.Tag is DriveInfo driveInfo)
            {
                return driveInfo.RootDirectory.FullName;
            }
            else if (node.Parent != null)
            {
                return GetLocalTargetPath(node.Parent);
            }

            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        private async Task UploadFileToFtp(string localPath, string remotePath)
        {
            try
            {
                string fileName = Path.GetFileName(localPath);
                string ftpUrl = $"ftp://{serverInfo.Ip}:{serverInfo.Port}/{remotePath}/{fileName}";

                var request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                //открывается файл для чтения
                using (FileStream fileStream = File.OpenRead(localPath))
                //открывается поток запроса на сервер
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    //содержимое файла копируется в поток запроса
                    await fileStream.CopyToAsync(requestStream);
                }

                MessageBox.Show("Файл успешно загружен на сервер!", "Успех");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки файла: {ex.Message}", "Ошибка");
            }
        }

        private async Task DownloadFileFromFtp(string remotePath, string localPath)
        {
            try
            {
                string fileName = Path.GetFileName(remotePath);
                string fullLocalPath = Path.Combine(localPath, fileName);

                string ftpUrl = $"ftp://{serverInfo.Ip}:{serverInfo.Port}/{remotePath}";

                var request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                //отправка запроса
                using (var response = (FtpWebResponse)await request.GetResponseAsync())
                //получение потока данных от сервера
                using (var responseStream = response.GetResponseStream())
                //создание локального файла для записи
                using (var fileStream = File.Create(fullLocalPath))
                {
                    await responseStream.CopyToAsync(fileStream);
                }

                MessageBox.Show("Файл успешно скачан!", "Успех");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка скачивания файла: {ex.Message}", "Ошибка");
            }
        }

        private async void btnAddFolder_Click(object sender, EventArgs e)
        {
            if (selectedNode == null)
            {
                MessageBox.Show("Выберите папку на сервере, куда хотите добавить новую папку.", "Выбор");
                return;
            }
            if (selectedNode?.Tag is bool isDirectory)
            {
                if (!isDirectory)
                {
                    MessageBox.Show("Выберите папку на сервере, куда хотите добавить новую папку.", "Выбор");
                    return;
                }
            }

            Form inputForm = new Form()
            {
                Width = 300,
                Height = 150,
                Text = "Новая папка",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label label = new Label() { Left = 10, Top = 15, Text = "Имя папки:", AutoSize = true };
            TextBox textBox = new TextBox() { Left = 10, Top = 35, Width = 260 };
            Button okButton = new Button() { Text = "Создать", Left = 115, Width = 75, Top = 70, DialogResult = DialogResult.OK };
            Button cancelButton = new Button() { Text = "Отмена", Left = 195, Width = 75, Top = 70, DialogResult = DialogResult.Cancel };

            inputForm.Controls.AddRange(new Control[] { label, textBox, okButton, cancelButton });
            inputForm.AcceptButton = okButton;
            inputForm.CancelButton = cancelButton;

            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                string folderName = textBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(folderName))
                {
                    MessageBox.Show("Имя папки не может быть пустым.", "Предупреждение");
                    return;
                }
                try
                {
                    string remotePath = GetFullRemotePath(selectedNode);
                    string newFolderPath = string.IsNullOrEmpty(remotePath)
                        ? folderName
                        : $"{remotePath}/{folderName}";

                    await CreateRemoteDirectoryRecursive(newFolderPath);
                    MessageBox.Show($"Папка \"{folderName}\" успешно создана!", "Успех");
                    LoadFtpInformation();
                }
                catch (WebException ex)
                {
                    MessageBox.Show("Ошибка создания папки: " + ex.Message, "Ошибка");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message, "Ошибка");
                }
            }
        }

        private async Task CreateRemoteDirectoryRecursive(string remotePath)
        {
            string[] pathParts = remotePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string currentPath = "";

            foreach (string part in pathParts)
            {
                currentPath = string.IsNullOrEmpty(currentPath) ? part : $"{currentPath}/{part}";

                try
                {
                    string ftpUrl = $"ftp://{serverInfo.Ip}:{serverInfo.Port}/{currentPath}";

                    var request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                    //запрос на создание папки
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    request.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                    request.UsePassive = true;
                    request.UseBinary = true;
                    request.KeepAlive = false;

                    try
                    {
                        using (var response = (FtpWebResponse)await request.GetResponseAsync()) { }
                    }
                    //если такая папка уже есть
                    catch (WebException ex) when ((ex.Response as FtpWebResponse)?.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось создать папку {currentPath}: {ex.Message}", "Ошибка");
                }
            }
        }

        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //в режиме перемещения и щёлкнули по папке
            if (isMoveMode && e.Node.Tag is bool isDir && isDir)
            {
                string targetDir = e.Node.FullPath.Replace("\\", "/").TrimStart('/');
                await MoveFileAsync(fileToMove, targetDir);

                btnDelete.Enabled = false;
                btnRename.Enabled = false;
                btnMove.Enabled = false;
                lblFile.Text = "Ничего не выбрано.";
                lblSizet.Text = "";
                lblModifiedt.Text = "";

                isMoveMode = false;
                fileToMove = null;
                LoadFtpInformation();
                return;
            }

            selectedNode = e.Node;
            if (selectedNode?.Tag is bool isDirectory)
            {
                if (!isDirectory)
                {
                    string filePath = GetFullRemotePath(selectedNode);
                    fileInfo = await GetFileInformation(filePath);

                    fileInfo = new FileInformation
                    {
                        FileName = Path.GetFileName(filePath),
                        FileSize = fileInfo.FileSize,
                        LastModified = fileInfo.LastModified,
                        DirectoryPath = Path.GetDirectoryName(filePath)?.Replace("\\", "/") ?? "/"
                    };

                    var elements = new Control[] { lblSize, lblModified, btnDelete, btnMove, btnRename };
                    foreach (var el in elements)
                    {
                        el.ForeColor = Color.Black;
                    }
                    btnDelete.Enabled = true;
                    btnRename.Enabled = true;
                    btnMove.Enabled = true;
                    lblFile.Text = "Файл: " + fileInfo.FileName;
                    lblSizet.Text = fileInfo.FileSize.ToString();
                    lblModifiedt.Text = fileInfo.LastModified.ToString();
                }
                else
                {
                    var dirInfo = await GetDirectoryInformation(GetFullRemotePath(selectedNode));

                    btnDelete.Enabled = true;
                    btnRename.Enabled = true; 
                    btnMove.Enabled = false;
                    var elements = new Control[] { lblSize, lblModified, btnDelete, btnMove, btnRename };
                    foreach (var el in elements)
                    {
                        el.ForeColor = Color.Black;
                    }
                    lblFile.Text = "Папка: " + selectedNode.Text;
                    lblSizet.Text = dirInfo.FileSize;
                    lblModifiedt.Text = dirInfo.LastModified;
                }
            }
            else
            {
                btnDelete.Enabled = true;
            }
        }

        private async Task<FileInformation> GetFileInformation(string filePath)
        {
            string ftpUrl = $"ftp://{serverInfo.Ip}:{serverInfo.Port}/{filePath}";
            string sizeFormatted = "N/A", lastModified = "N/A";

            try
            {
                //получение размера файла
                var sizeRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
                sizeRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                sizeRequest.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                sizeRequest.UsePassive = true;
                sizeRequest.UseBinary = true;
                sizeRequest.KeepAlive = false;

                using (var sizeResponse = (FtpWebResponse)await sizeRequest.GetResponseAsync())
                {
                    long size = sizeResponse.ContentLength;
                    sizeFormatted = FormatFileSize(size);
                }

                //получение даты последнего изменения файла
                var dateRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
                dateRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                dateRequest.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                dateRequest.UsePassive = true;
                dateRequest.UseBinary = true;
                dateRequest.KeepAlive = false;

                using (var dateResponse = (FtpWebResponse)await dateRequest.GetResponseAsync())
                {
                    lastModified = dateResponse.LastModified.ToString("dd-MM-yyyy HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных файла: {ex.Message}", "Ошибка");
            }
            return new FileInformation
            {
                FileName = filePath,
                FileSize = sizeFormatted,
                LastModified = lastModified,
                DirectoryPath = Path.GetDirectoryName(filePath).Replace("\\", "/")
            };
        }

        private string FormatFileSize(long sizeInBytes)
        {
            if (sizeInBytes < 1024)
                return $"{sizeInBytes} Bytes";
            else if (sizeInBytes < 1048576)
                return $"{sizeInBytes / 1024} KB";
            else if (sizeInBytes < 1073741824)
                return $"{sizeInBytes / 1048576} MB";
            else
                return $"{sizeInBytes / 1073741824} GB";
        }

        private async Task<FileInformation> GetDirectoryInformation(string path)
        {
            try
            {
                string ftpUrl = $"ftp://{serverInfo.Ip}:{serverInfo.Port}/{path}";
                string lastModified = "N/A";
                long size = 0;

                try
                {

                    var dateRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
                    dateRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                    dateRequest.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                    dateRequest.UsePassive = true;
                    dateRequest.UseBinary = true;
                    dateRequest.KeepAlive = false;

                    using (var response = (FtpWebResponse)await dateRequest.GetResponseAsync())
                    {
                        lastModified = response.LastModified.ToString("dd-MM-yyyy HH:mm:ss");
                    }
                }
                catch { }
                try
                {
                    size = await CalculateDirectorySize(path);
                }
                catch { }

                return new FileInformation
                {
                    FileName = Path.GetFileName(path),
                    FileSize = size > 0 ? FormatFileSize(size) : "N/A",
                    LastModified = lastModified,
                    DirectoryPath = Path.GetDirectoryName(path)?.Replace("\\", "/") ?? "/"
                };
            }
            catch (Exception)
            {
                return new FileInformation
                {
                    FileName = Path.GetFileName(path),
                    FileSize = "N/A",
                    LastModified = "N/A",
                    DirectoryPath = Path.GetDirectoryName(path)?.Replace("\\", "/") ?? "/"
                };
            }
        }

        private async Task<long> CalculateDirectorySize(string path)
        {
            long totalSize = 0;
            try
            {
                string ftpUrl = $"ftp://{serverInfo.Ip}:{serverInfo.Port}/{path}";

                var listRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
                listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                listRequest.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                listRequest.UsePassive = true;
                listRequest.UseBinary = true;
                listRequest.KeepAlive = false;

                //отправка запроса
                using (var response = (FtpWebResponse)await listRequest.GetResponseAsync())
                using (var stream = response.GetResponseStream())
                //построчное считывание строк
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = await reader.ReadLineAsync();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string name = ExtractName(line);
                        bool isDir = IsDirectory(line);
                        //пропуск текущей, родительской папок
                        if (name == "." || name == "..") continue;

                        string itemPath = $"{path}/{name}";

                        //рекурсивный вызов для подпапки
                        if (isDir)
                        {
                            try
                            {
                                totalSize += await CalculateDirectorySize(itemPath);
                            }
                            catch
                            {
                                //пропуск папок к которым нет доступа
                                continue;
                            }
                        }
                        else
                        {
                            try
                            {
                                var sizeRequest = (FtpWebRequest)WebRequest.Create($"ftp://{serverInfo.Ip}:{serverInfo.Port}/{itemPath}");
                                sizeRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                                sizeRequest.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                                sizeRequest.UsePassive = true;
                                sizeRequest.UseBinary = true;
                                sizeRequest.KeepAlive = false;

                                using (var sizeResponse = (FtpWebResponse)await sizeRequest.GetResponseAsync())
                                {
                                    totalSize += sizeResponse.ContentLength;
                                }
                            }
                            catch
                            {
                                //пропуск файлов к которым нет доступа
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вычислении размера папки: {ex.Message}", "Ошибка");
            }

            return totalSize;
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            selectedNode = trvServer?.SelectedNode;

            if (selectedNode == null || serverInfo == null)
                return;

            if (selectedNode.Tag is bool isDirectory)
            {
                string itemName = selectedNode.Text;
                string itemType = isDirectory ? "папку" : "файл";
                string path = GetFullRemotePath(selectedNode);

                var confirm = MessageBox.Show(
                    $"Вы уверены, что хотите удалить {itemType} \"{itemName}\"?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm != DialogResult.Yes)
                    return;

                try
                {
                    if (isDirectory)
                    {
                        await DeleteDirectoryRecursive(path);
                    }
                    else
                    {
                        await DeleteFile(path);
                    }

                    MessageBox.Show(isDirectory ? "Папка успешно удалена." : "Файл успешно удален.", "Успех");

                    LoadFtpInformation();
                    fileInfo = null;
                    lblFile.Text = "Ничего не выбрано";
                    lblSizet.Text = "";
                    lblModifiedt.Text = "";
                    btnDelete.Enabled = false;
                    btnRename.Enabled = false;
                    btnMove.Enabled = false;
                    var elements = new Control[] { lblSize, lblModified, btnDelete, btnMove, btnRename };
                    foreach (var el in elements)
                    {
                        el.ForeColor = Color.Gray;
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show($"Ошибка FTP: {ex.Message}", "Ошибка");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Выберите файл или папку на FTP-сервере для удаления.", "Информация");
            }
        }

        private async Task DeleteDirectoryRecursive(string path)
        {
            try
            {
                string ftpUrl = $"ftp://{serverInfo.Ip}:{serverInfo.Port}/{path}";

                //удаление содержимого папки
                var listRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
                listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                listRequest.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                listRequest.UsePassive = true;
                listRequest.UseBinary = true;
                listRequest.KeepAlive = false;

                using (var response = (FtpWebResponse)await listRequest.GetResponseAsync())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        //считывание каждой строки из ответа - файл или папка
                        string line = await reader.ReadLineAsync();
                        string name = ExtractName(line);
                        bool isDir = IsDirectory(line);

                        if (name == "." || name == "..") continue;

                        string itemPath = $"{path}/{name}";

                        if (isDir)
                        {
                            await DeleteDirectoryRecursive(itemPath);
                        }
                        else
                        {
                            await DeleteFile(itemPath);
                        }
                    }
                }

                var deleteRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
                deleteRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                deleteRequest.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                deleteRequest.UsePassive = true;
                deleteRequest.UseBinary = true;
                deleteRequest.KeepAlive = false;

                using (var deleteResponse = (FtpWebResponse)await deleteRequest.GetResponseAsync()) { }
            }
            catch (Exception ex)
            {
               MessageBox.Show($"Не удалось удалить папку {path}: {ex.Message}", "Ошибка");
            }
        }

        private async Task DeleteFile(string path)
        {
            string ftpUrl = $"ftp://{serverInfo.Ip}:{serverInfo.Port}/{path}";

            var request = (FtpWebRequest)WebRequest.Create(ftpUrl);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            using (var response = (FtpWebResponse)await request.GetResponseAsync()) { }
        }

        private async void btnRename_Click(object sender, EventArgs e)
        {
            selectedNode = trvServer?.SelectedNode;

            if (selectedNode == null)
            {
                MessageBox.Show("Выберите файл или папку для переименования.", "Предупреждение");
                return;
            }

            string currentName = selectedNode.Text;
            string currentPath = GetFullRemotePath(selectedNode);
            bool isDirectory = selectedNode.Tag is bool dirFlag && dirFlag;

            Form renameForm = new Form()
            {
                Width = 300,
                Height = 150,
                Text = "Переименовать",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label label = new Label() { Left = 10, Top = 15, Text = "Новое имя:", AutoSize = true };
            TextBox textBox = new TextBox() { Left = 10, Top = 35, Width = 260, Text = currentName };
            Button okButton = new Button() { Text = "Oк", Left = 115, Width = 75, Top = 70, DialogResult = DialogResult.OK };
            Button cancelButton = new Button() { Text = "Отмена", Left = 195, Width = 75, Top = 70, DialogResult = DialogResult.Cancel };

            renameForm.Controls.AddRange(new Control[] { label, textBox, okButton, cancelButton });
            renameForm.AcceptButton = okButton;
            renameForm.CancelButton = cancelButton;

            if (renameForm.ShowDialog() == DialogResult.OK)
            {
                string newName = textBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(newName))
                {
                    MessageBox.Show("Имя не может быть пустым.", "Ошибка");
                    return;
                }

                try
                {
                    string newPath = Path.GetDirectoryName(currentPath) + "/" + newName;
                    newPath = newPath.Replace("\\", "/").TrimStart('/');

                    string ftpUrl = $"ftp://{serverInfo.Ip}:{serverInfo.Port}/{currentPath}";

                    var request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                    request.Method = WebRequestMethods.Ftp.Rename;
                    request.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                    request.UseBinary = true;
                    request.UsePassive = true;
                    request.KeepAlive = false;
                    request.RenameTo = newName;

                    using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                    {
                        MessageBox.Show($"{(isDirectory ? "Папка успешно переименована на " : "Файл успешно переименован на ")} {newName}.", "Успех");
                    }

                    LoadFtpInformation();

                    lblFile.Text = "Ничего не выбрано";
                    lblSizet.Text = "";
                    lblModifiedt.Text = "";
                    btnDelete.Enabled = false;
                    btnRename.Enabled = false;
                    btnMove.Enabled = false;
                    var elements = new Control[] { lblSize, lblModified, btnDelete, btnMove, btnRename };
                    foreach (var el in elements)
                    {
                        el.ForeColor = Color.Gray;
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show($"FTP ошибка: {ex.Message}", "Ошибка");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка переименования: {ex.Message}", "Ошибка");
                }
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            if (fileInfo == null)
            {
                MessageBox.Show("Выберите файл для перемещения.", "Выбор");
                return;
            }

            fileToMove = fileInfo;
            isMoveMode = true;
            MessageBox.Show(
                "Теперь выберите папку на сервере, куда хотите переместить файл.",
                "Перемещение",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private async Task MoveFileAsync(FileInformation srcFile, string remoteTargetDir)
        {
            try
            {
                remoteTargetDir = remoteTargetDir.Replace("localhost/", "").Trim('/');

                string oldPath = $"/{srcFile.DirectoryPath.Trim('/')}/{srcFile.FileName}".Replace("//", "/");
                string newPath = $"/{remoteTargetDir}/{srcFile.FileName}".Replace("//", "/");


                string ftpUrl = $"ftp://{serverInfo.Ip}:{serverInfo.Port}/{oldPath}";
                var moveReq = (FtpWebRequest)WebRequest.Create(ftpUrl);
                moveReq.Method = WebRequestMethods.Ftp.Rename;
                moveReq.Credentials = new NetworkCredential(serverInfo.UserName, serverInfo.Password);
                moveReq.UsePassive = true;
                moveReq.UseBinary = true;
                moveReq.KeepAlive = false;

                moveReq.RenameTo = newPath;

                using (var resp = (FtpWebResponse)await moveReq.GetResponseAsync())
                {
                    MessageBox.Show($"Файл перемещен в {newPath}", "Успех");
                }
            }
            catch (WebException ex) when (ex.Response is FtpWebResponse ftpResp)
            {
                MessageBox.Show($"FTP ошибка: {ftpResp.StatusDescription.Trim()}", "Ошибка");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка перемещения файла: " + ex.Message, "Ошибка");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadFtpInformation();
        }
    }

    public class FtpServerInfo
    {
        public string Ip { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class FileInformation
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string LastModified { get; set; }
        public string DirectoryPath { get; set; }
    }
}
