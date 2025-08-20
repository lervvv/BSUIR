using Newtonsoft.Json;
using System;

namespace FileStorageGraphQLClient
{
    //для сопоставления ответа сервера и поля класса
    public class FileEntry
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("isDirectory")]
        public bool IsDirectory { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("lastModified")]
        public DateTime LastModified { get; set; }
    }
}