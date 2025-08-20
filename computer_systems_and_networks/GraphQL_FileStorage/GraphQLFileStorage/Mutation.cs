using GraphQLFileStorage.Services;

public class Mutation
{
    [GraphQLName("deleteFile")] //явное указание метода в GraphQL
    public bool DeleteFile(
        [GraphQLName("path")] string path, //ожидание передачи пути
        [Service] FileStorageService service //внедрение сервиса автоматически
    ) => service.Delete(path); //вызов метода удаления


    [GraphQLName("appendToFile")]
    public bool AppendToFile(
        [GraphQLName("path")] string path,
        [GraphQLName("content")] string content,
        [Service] FileStorageService service
    ) => service.AppendToFile(path, content);

   
    [GraphQLName("overwriteFile")]
    public bool OverwriteFile(
    [GraphQLName("path")] string path,
    [GraphQLName("content")] string content,
    [Service] FileStorageService service
    ) => service.OverwriteFile(path, content);

    
    [GraphQLName("copyFile")]
    public bool CopyFile(
    [GraphQLName("source")] string sourcePath,
    [GraphQLName("destination")] string destinationPath,
    [Service] FileStorageService service
    ) => service.CopyFile(sourcePath, destinationPath);

    
    [GraphQLName("moveFile")]
    public bool MoveFile(
    [GraphQLName("source")] string sourcePath,
    [GraphQLName("destination")] string destinationPath,
    [Service] FileStorageService service
    ) => service.MoveFile(sourcePath, destinationPath);
}