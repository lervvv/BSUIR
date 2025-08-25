using GraphQLFileStorage;
using GraphQLFileStorage.Services;

var builder = WebApplication.CreateBuilder(args);

//регистрация сервисов
builder.Services.AddSingleton<FileStorageService>();
builder.Services.AddControllers();

//настройка GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<FileItem>()
    .AddType<FileContentType>()
    .AddType<UploadType>();

var app = builder.Build();

//проверка ключа для входа
app.Use(async (context, next) =>
{
    const string apiKey = "123";
    var requestKey = context.Request.Headers["X-Api-Key"].ToString();

    if (string.IsNullOrEmpty(requestKey))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("API Key is missing");
        return;
    }
    if (requestKey != apiKey)
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Invalid API Key");
        return;
    }

    await next.Invoke();
});

// Конфигурация middleware
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL(); //подключение GraphQL 
    endpoints.MapControllers();
});

app.Run();