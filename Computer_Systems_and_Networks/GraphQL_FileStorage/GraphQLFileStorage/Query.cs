using GraphQLFileStorage.Services;

namespace GraphQLFileStorage
{
    public class Query
    {
        [GraphQLName("getFiles")] //явное указание метода в GraphQL
        public IEnumerable<FileItem> GetFiles([Service] FileStorageService service, string? path = null) //с передачей сервиса 
            => service.GetFiles(path ?? "");

        [GraphQLName("getFileContent")]
        public FileContent GetFileContent([Service] FileStorageService service, string path) => new FileContent
        {
            Path = path,
            Content = service.ReadFileContent(path)
        };
    }
}