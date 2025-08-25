using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;

namespace FileStorageGraphQLClient
{
    class Program
    {
        //создание, инициализация клиента для работы с GraphQL
        private static readonly GraphQLHttpClient _client = new GraphQLHttpClient(
            "https://localhost:7187/graphql",
            new NewtonsoftJsonSerializer());

        static async Task Main(string[] args)
        {
            _client.HttpClient.DefaultRequestHeaders.Add("X-Api-Key", "123");
            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1 - Просмотреть файлы");
                Console.WriteLine("2 - Прочитать файл"); 
                Console.WriteLine("3 - Удалить файл");
                Console.WriteLine("4 - Копировать файл"); 
                Console.WriteLine("5 - Добавить в конец файла");
                Console.WriteLine("6 - Перезаписать файл");
                Console.WriteLine("7 - Переместить файл");
                Console.WriteLine("0 - Выход");
                Console.Write("Ваш выбор: ");
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await ListFiles();
                            break;
                        case "2":
                            await ReadFile();
                            break;
                        case "3":
                            await DeleteFile();
                            break;
                        case "4":
                            await CopyFile();
                            break;
                        case "5":
                            await AppendToFile();
                            break;
                        case "6":
                            await OverwriteFile();
                            break;
                        case "7":
                            await MoveFile();
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

        private static async Task ListFiles()
        {
            Console.Write("Введите путь (оставьте пустым для корня): ");
            var path = Console.ReadLine();
            //формирование запроса с переменной пути
            var query = new GraphQLRequest //объект, кот содержит строку запроса и переменные
            {
                Query = @"
            query ($path: String) {
                getFiles(path: $path) {
                    name
                    path
                    isDirectory
                    size
                    lastModified
                }
            }",
                Variables = new { path = string.IsNullOrWhiteSpace(path) ? null : path } //аргумент, который пердаем как путь
            };
            //отправка запроса на сервер
            var response = await _client.SendQueryAsync<FilesResponse>(query);

            if (response.Errors != null)
            {
                Console.WriteLine($"Ошибка при получении списка файлов.\n");
                return;
            }
            foreach (var file in response.Data.GetFiles)
            {
                Console.WriteLine($"{(file.IsDirectory ? "[D] " : "[F] ")}{file.Name} " +
                    $"(Путь: {file.Path}, Размер: {file.Size} байт, Изменен: {file.LastModified:g})");
            }
        }

        private static async Task ReadFile()
        {
            Console.Write("Введите путь к файлу: ");
            var path = Console.ReadLine();
            //формирование запроса с переменной пути
            var query = new GraphQLRequest //объект, кот содержит строку запроса и переменные
            {
                Query = @"
            query ($path: String!) {
                getFileContent(path: $path) {
                    content
                }
            }",
                Variables = new { path } //аргумент, который передаем как путь
            };
            try
            {
                var response = await _client.SendQueryAsync<FileContentResponse>(query);

                if (response.Data?.GetFileContent == null)
                {
                    Console.WriteLine("Файл не найден.\n");
                    return;
                }

                Console.WriteLine($"Содержимое файла:\n{response.Data.GetFileContent.Content}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }

        private static async Task DeleteFile()
        {
            Console.Write("Введите путь к файлу: ");
            var path = Console.ReadLine();

            var request = new GraphQLRequest
            {
                Query = @"
            mutation ($path: String!) {
                deleteFile(path: $path)
            }",
                Variables = new { path }
            };

            var response = await _client.SendMutationAsync<DeleteFileResponse>(request);

            Console.WriteLine(response.Data.DeleteFile
                ? "Удаление успешно выполнено.\n"
                : "Ошибка удаления.\n");
        }

        private static async Task AppendToFile()
        {
            Console.Write("Введите путь к файлу: ");
            var path = Console.ReadLine();

            Console.Write("Введите текст для добавления: ");
            var text = Console.ReadLine();

            var request = new GraphQLRequest
            {
                Query = @"
            mutation ($path: String!, $content: String!) {
                appendToFile(path: $path, content: $content)
            }",
                Variables = new { path, content = text }
            };

            var response = await _client.SendMutationAsync<AppendToFileResponse>(request);

            Console.WriteLine(response.Data.AppendToFile
                ? "Текст успешно добавлен.\n"
                : "Ошибка при добавлении.\n");
        }

        private static async Task OverwriteFile()
        {
            Console.Write("Введите путь к файлу: ");
            var path = Console.ReadLine();

            Console.Write("Введите новый текст: ");
            var text = Console.ReadLine();

            var request = new GraphQLRequest
            {
                Query = @"
            mutation ($path: String!, $content: String!) {
                overwriteFile(path: $path, content: $content)
            }",
                Variables = new { path, content = text }
            };

            var response = await _client.SendMutationAsync<OverwriteFileResponse>(request);

            Console.WriteLine(response.Data.OverwriteFile
                ? "Файл успешно перезаписан.\n"
                : "Ошибка при перезаписи.\n");
        }

        private static async Task CopyFile()
        {
            Console.Write("Введите исходный путь: ");
            var source = Console.ReadLine();

            Console.Write("Введите путь назначения: ");
            var destination = Console.ReadLine();

            var request = new GraphQLRequest
            {
                Query = @"
            mutation ($source: String!, $destination: String!) {
                copyFile(source: $source, destination: $destination)
            }",
                Variables = new { source, destination }
            };

            var response = await _client.SendMutationAsync<CopyFileResponse>(request);

            Console.WriteLine(response.Data.CopyFile
                ? "Файл успешно скопирован.\n"
                : "Ошибка копирования.\n");
        }

        private static async Task MoveFile()
        {
            Console.Write("Введите исходный путь: ");
            var source = Console.ReadLine();

            Console.Write("Введите путь назначения: ");
            var destination = Console.ReadLine();

            var request = new GraphQLRequest
            {
                Query = @"
            mutation ($source: String!, $destination: String!) {
                moveFile(source: $source, destination: $destination)
            }",
                Variables = new { source, destination }
            };

            var response = await _client.SendMutationAsync<MoveFileResponse>(request);

            Console.WriteLine(response.Data.MoveFile
                ? "Файл успешно перемещен.\n"
                : "Ошибка перемещения.\n");
        }
    }

    //модели для десериализации ответа
    public class FilesResponse
    {
        [JsonProperty("getFiles")]
        public List<FileEntry> GetFiles { get; set; }
    }

    public class FileContentResponse
    {
        [JsonProperty("getFileContent")] //сервер возвращает данные в Json формате 
        //превращение Json в объект C#
        public FileContent GetFileContent { get; set; }
    }

    public class DeleteFileResponse
    {
        [JsonProperty("deleteFile")]
        public bool DeleteFile { get; set; } //свойство показывает, успешна ли операция
    }

    public class AppendToFileResponse
    {
        [JsonProperty("appendToFile")]
        public bool AppendToFile { get; set; }
    }

    public class OverwriteFileResponse
    {
        [JsonProperty("overwriteFile")]
        public bool OverwriteFile { get; set; }
    }

    public class CopyFileResponse
    {
        [JsonProperty("copyFile")]
        public bool CopyFile { get; set; }
    }

    public class MoveFileResponse
    {
        [JsonProperty("moveFile")]
        public bool MoveFile { get; set; }
    }

    public class FileContent
    {
        public string Path { get; set; }
        public string Content { get; set; }
    }
}