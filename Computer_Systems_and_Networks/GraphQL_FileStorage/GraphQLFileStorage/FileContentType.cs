namespace GraphQLFileStorage
{
    //определение типа содержимого файла
    public class FileContent
    {
        public string Path { get; set; }
        public string Content { get; set; }
    }
    //метод для построения схемы GraphQL
    public class FileContentType : ObjectType<FileContent>
    {
        protected override void Configure(IObjectTypeDescriptor<FileContent> descriptor)
        {
            descriptor.Field(f => f.Path).Type<StringType>(); //включение поля пути в граф
            descriptor.Field(f => f.Content).Type<StringType>();
        }
    }
}