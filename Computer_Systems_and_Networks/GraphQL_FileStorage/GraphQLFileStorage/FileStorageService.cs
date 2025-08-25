using System.Text;

namespace GraphQLFileStorage.Services
{
    //сервис для работы с системой файлов
    public class FileStorageService
    {
        private readonly string _storagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Storage");

        public FileStorageService()
        {
            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }

        public IEnumerable<FileItem> GetFiles(string relativePath = "")
        {
            var fullPath = System.IO.Path.Combine(_storagePath, relativePath);
            if (!Directory.Exists(fullPath)) return Enumerable.Empty<FileItem>();

            return new DirectoryInfo(fullPath)
                .GetFileSystemInfos() //возвращает массив объектов с файлами и папками
                .Select(f => new FileItem
                {
                    Id = f.Name,
                    Name = f.Name,
                    Path = System.IO.Path.Combine(relativePath, f.Name),
                    IsDirectory = f is DirectoryInfo,
                    Size = f is FileInfo fi ? fi.Length : 0,
                    LastModified = f.LastWriteTimeUtc
                });
        }

        public string ReadFileContent(string id)
        {
            var fullPath = System.IO.Path.Combine(_storagePath, id);
            return File.ReadAllText(fullPath, Encoding.UTF8);
        }

        public bool Delete(string id)
        {
            var fullPath = System.IO.Path.Combine(_storagePath, id);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, recursive: true);
                return true;
            }
            return false;
        }

        public bool AppendToFile(string path, string content)
        {
            var fullPath = System.IO.Path.Combine(_storagePath, path);
            if (!File.Exists(fullPath))
                return false;
            File.AppendAllText(fullPath, content, Encoding.UTF8);
            return true;
        }

        public bool OverwriteFile(string path, string content)
        {
            var fullPath = System.IO.Path.Combine(_storagePath, path);
            File.WriteAllText(fullPath, content, Encoding.UTF8);
            return true;
        }

        public bool CopyFile(string sourcePath, string destinationPath)
        {
            var sourceFullPath = System.IO.Path.Combine(_storagePath, sourcePath);
            var destinationFullPath = System.IO.Path.Combine(_storagePath, destinationPath);

            if (!File.Exists(sourceFullPath))
                return false;

            if (File.Exists(destinationFullPath))
                return false;

            File.Copy(sourceFullPath, destinationFullPath);
            return true;
        }

        public bool MoveFile(string sourcePath, string destinationPath)
        {
            var sourceFullPath = System.IO.Path.Combine(_storagePath, sourcePath);
            var destinationFullPath = System.IO.Path.Combine(_storagePath, destinationPath);

            if (!File.Exists(sourceFullPath))
                return false;

            if (File.Exists(destinationFullPath))
                return false;

            File.Move(sourceFullPath, destinationFullPath);
            return true;
        }
    }
}