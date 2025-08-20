namespace GraphQLFileStorage
{
    //модель данных для представления файла или папки в системе
    public class FileItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool IsDirectory { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }
}