using System.Text;
using System.Text.Json;

namespace FileStorageClient
{
    internal class Program
    {
        private static readonly string baseAddress = "https://localhost:7048/api/files";

        static async Task Main(string[] args)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            var client = new HttpClient(handler);

            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1 - Просмотреть все файлы");
                Console.WriteLine("2 - Работа с файлом");
                Console.WriteLine("3 - Добавить новый файл");
                Console.WriteLine("4 - Создать папку");
                Console.WriteLine("5 - Удалить папку");
                Console.WriteLine("0 - Выход");
                Console.Write("Ваш выбор: ");
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await ListTree(client);
                            break;
                        case "2":
                            await FileOperationsMenu(client);
                            break;
                        case "3":
                            await UploadFile(client);
                            break;
                        case "4":
                            await CreateDirectory(client);
                            break;
                        case "5":
                            await DeleteFolder(client);
                            break;
                        case "0":
                            Console.WriteLine("Выход.");
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        private static async Task FileOperationsMenu(HttpClient client)
        {
            Console.Write("Введите путь к файлу на сервере: ");
            var serverFilePath = Console.ReadLine()?.Trim('/', ' ');

            var checkResponse = await client.GetAsync($"{baseAddress}/{serverFilePath}");
            if (!checkResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Файл не найден.");
                return;
            }

            string choice;
            do
            {
                Console.WriteLine("Работа с файлом:");
                Console.WriteLine("1 - Прочитать файл");
                Console.WriteLine("2 - Добавить в конец файла");
                Console.WriteLine("3 - Копировать файл");
                Console.WriteLine("4 - Скачать файл");
                Console.WriteLine("5 - Удалить файл");
                Console.WriteLine("6 - Переместить файл");
                Console.WriteLine("0 - Назад");
                Console.Write("Ваш выбор: ");
                choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": await ReadFile(client, serverFilePath); break;
                        case "2": await AppendToFile(client, serverFilePath); break;
                        case "3": await CopyFile(client, serverFilePath); break;
                        case "4": await DownloadFile(client, serverFilePath); break;
                        case "5": await DeleteFile(client, serverFilePath); choice = "0"; break;
                        case "6": await MoveFile(client, serverFilePath);choice = "0"; break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            } while (choice != "0");
        }

        private static async Task ListTree(HttpClient client)
        {
            var response = await client.GetAsync(baseAddress); //отправка GET запроса 
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Не удалось получить список файлов.\n\n");
                return;
            }
            
            var json = await response.Content.ReadAsStringAsync(); //считывание ответа
            var entries = JsonSerializer.Deserialize<List<EntryDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (entries == null || entries.Count == 0)
            {
                Console.WriteLine("Хранилище пусто.\n\n");
                return;
            }

            var tree = BuildTree(entries);

            void Print(IEnumerable<FileNode> list, string indent)
            {
                foreach (var node in list)
                {
                    Console.WriteLine($"{indent}{(node.IsDirectory ? "[D] " : "    ")}{node.Name}");
                    if (node.IsDirectory && node.Children != null)
                        Print(node.Children, indent + "  ");
                }
            }
            Print(tree, ""); //функция рекурсивно печатает дерево
            Console.WriteLine("\n\n");
        }

        private static List<FileNode> BuildTree(List<EntryDto> entries)
        {
            var root = new FileNode { Name = "/", IsDirectory = true, Children = new List<FileNode>() }; //корневая папка

            foreach (var entry in entries)
            {
                var parts = entry.Path.Split('/', StringSplitOptions.RemoveEmptyEntries); //разделение пути с помощью /
                var current = root;

                for (int i = 0; i < parts.Length; i++) //проход по всем частям пути 
                {
                    var part = parts[i];
                    bool isLast = i == parts.Length - 1;

                    var found = current.Children.Find(c => c.Name == part); //проверка есть ли такой элемент в текущем узле
                    if (found == null)
                    {
                        var newNode = new FileNode
                        {
                            Name = part,
                            IsDirectory = isLast ? entry.IsDirectory : true, //папка или файл
                            Children = isLast && !entry.IsDirectory ? null : new List<FileNode>() //нужен ли дочений список
                        };
                        current.Children.Add(newNode);
                        current = newNode;
                    }
                    else
                    {
                        current = found;
                    }
                }
            }
            return root.Children;
        }

        private static async Task UploadFile(HttpClient client)
        {
            Console.Write("Введите путь к локальному файлу: ");
            var localPath = Console.ReadLine();
            if (!File.Exists(localPath))
            {
                Console.WriteLine("Файл не найден.\n\n");
                return;
            }
            Console.Write("Введите путь на сервере: ");
            var serverPath = Console.ReadLine()?.Trim('/', ' ');
            if (string.IsNullOrEmpty(serverPath))
            {
                Console.WriteLine("Путь не задан.\n\n");
                return;
            }
            using var fs = File.OpenRead(localPath); //возвращает объект, через который можно читать файл
            var response = await client.PutAsync($"{baseAddress}/{serverPath}", new StreamContent(fs));

            Console.WriteLine(response.IsSuccessStatusCode ? "Файл загружен.\n\n" : "Ошибка загрузки.\n\n");
        }

        private static async Task ReadFile(HttpClient client, string serverFilePath)
        {
            var response = await client.GetAsync($"{baseAddress}/{serverFilePath}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Ошибка чтения.\n\n");
                return;
            }
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("\nСодержимое файла:\n" + content + "\n\n");
        }

        private static async Task AppendToFile(HttpClient client, string serverFilePath)
        {
            Console.Write("Введите текст: ");
            var textToAppend = Console.ReadLine();
            if (string.IsNullOrEmpty(textToAppend))
            {
                Console.WriteLine("Пустой ввод.\n\n");
                return;
            }
            var content = new StringContent(textToAppend, Encoding.UTF8, "text/plain"); //объект, содержащий данные, отправляемые на сервер
            var response = await client.PostAsync($"{baseAddress}/append/{serverFilePath}", content);

            Console.WriteLine(response.IsSuccessStatusCode ? "Текст добавлен.\n\n" : "Ошибка добавления.\n\n");
        }

        private static async Task CopyFile(HttpClient client, string sourcePath)
        {
            Console.Write("Введите путь назначения: ");
            var destinationPath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(destinationPath))
            {
                Console.WriteLine("Путь не задан.\n\n");
                return;
            }
            //формирует строку URL для отправки запроса на сервер
            var url = $"{baseAddress}/copy?source={Uri.EscapeDataString(sourcePath)}&destination={Uri.EscapeDataString(destinationPath)}";
            var response = await client.PostAsync(url, null);
            Console.WriteLine(response.IsSuccessStatusCode ? "Файл скопирован.\n\n" : "Ошибка копирования.\n\n");
        }

        private static async Task DownloadFile(HttpClient client, string serverFilePath)
        {
            Console.Write("Введите путь для сохранения: ");
            var localSavePath = Console.ReadLine();
            if (string.IsNullOrEmpty(localSavePath))
            {
                Console.WriteLine("Путь не задан.\n\n");
                return;
            }
            var url = $"{baseAddress}/download?path={Uri.EscapeDataString(serverFilePath)}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Ошибка скачивания.\n\n");
                return;
            }
            //считывание данных файла
            var data = await response.Content.ReadAsByteArrayAsync();
            //запись данных на локальный конмпьютер
            await File.WriteAllBytesAsync(localSavePath, data);
            Console.WriteLine("Файл скачан.\n\n");
        }

        private static async Task DeleteFile(HttpClient client, string serverFilePath)
        {
            var url = $"{baseAddress}/delete?path={Uri.EscapeDataString(serverFilePath)}";
            var response = await client.DeleteAsync(url);
            Console.WriteLine(response.IsSuccessStatusCode ? "Файл удалён.\n\n" : "Ошибка удаления.\n\n");
        }

        private static async Task MoveFile(HttpClient client, string sourcePath)
        {
            Console.Write("Введите новый путь: ");
            var destinationPath = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(destinationPath))
            {
                Console.WriteLine("Путь не задан.\n\n");
                return;
            }
            var url = $"{baseAddress}/move?source={Uri.EscapeDataString(sourcePath)}&destination={Uri.EscapeDataString(destinationPath)}";
            var response = await client.PostAsync(url, null);

            Console.WriteLine(response.IsSuccessStatusCode ? "Файл перемещён.\n\n" : "Ошибка перемещения.\n\n");
        }

        private static async Task CreateDirectory(HttpClient client)
        {
            Console.Write("Введите путь новой папки: ");
            var newFolder = Console.ReadLine()?.Trim('/', ' ');
            if (string.IsNullOrEmpty(newFolder))
            {
                Console.WriteLine("Путь не задан.\n\n");
                return;
            }
            var response = await client.PostAsync($"{baseAddress}/mkdir/{newFolder}", null);

            if (response.IsSuccessStatusCode)
                Console.WriteLine(await response.Content.ReadAsStringAsync() + "\n\n");
            else
                Console.WriteLine("Ошибка при создании папки.\n\n");
        }
        private static async Task DeleteFolder(HttpClient client)
        {
            Console.Write("Введите путь к папке: ");
            var folderPath = Console.ReadLine()?.Trim('/', ' ');
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                Console.WriteLine("Путь не задан.\n\n");
                return;
            }
            Console.Write("Удалить папку и содержимое? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y")
            {
                Console.WriteLine("Операция отменена.\n\n");
                return;
            }
            var url = $"{baseAddress}/delete-folder?path={Uri.EscapeDataString(folderPath)}";
            var response = await client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Папка удалена.\n\n");
            else
                Console.WriteLine("Ошибка удаления.\n\n");
        }

        // DTO-классы
        private class FileNode
        {
            public string Name { get; set; }
            public bool IsDirectory { get; set; }
            public List<FileNode> Children { get; set; }
        }

        private class EntryDto
        {
            public string Path { get; set; }
            public bool IsDirectory { get; set; }
        }
    }
}
