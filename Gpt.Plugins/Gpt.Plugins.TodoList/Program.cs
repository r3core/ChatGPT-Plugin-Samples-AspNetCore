using System.Text.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(
    options => options.AddDefaultPolicy(corsPolicyBuilder => corsPolicyBuilder.WithOrigins("https://chat.openai.com")));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost(
        pattern: "/todos/{username}",
        (string username, TodoItem todo, HttpContext context) =>
        {
            var todos = context.Items.GetOrCreate<List<TodoItem>>(username);
            todos.Add(todo);
            context.Response.StatusCode = StatusCodes.Status200OK;
        })
   .WithName("AddTodo")
   .WithOpenApi();

app.MapGet(
        pattern: "/todos/{username}",
        (string username, HttpContext context) =>
        {
            var todos = context.Items.GetOrCreate<List<TodoItem>>(username);
            context.Response.ContentType = "application/json";

            return JsonSerializer.Serialize(todos);
        })
   .WithName("GetTodos")
   .WithOpenApi();

app.MapDelete(
        pattern: "/todos/{username}",
        (string username, int todoIdx, HttpContext context) =>
        {
            var todos = context.Items.GetOrCreate<List<TodoItem>>(username);

            if (0 <= todoIdx
             && todoIdx < todos.Count)
            {
                todos.RemoveAt(todoIdx);
            }

            context.Response.StatusCode = StatusCodes.Status200OK;
        })
   .WithName("DeleteTodo")
   .WithOpenApi();

app.MapGet(pattern: "/logo.png", () => Results.File(path: "logo.png", contentType: "image/png"))
   .WithName("PluginLogo")
   .WithOpenApi();

app.MapGet(
        pattern: "/.well-known/ai-plugin.json",
        async context =>
        {
            string host = context.Request.Host.ToString();
            string text = await File.ReadAllTextAsync("manifest.json");
            text = text.Replace(oldValue: "PLUGIN_HOSTNAME", $"https://{host}");
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(text);
        })
   .WithName("PluginManifest")
   .WithOpenApi();

app.MapGet(
        pattern: "/openapi.yaml",
        async context =>
        {
            string host = context.Request.Host.ToString();
            string text = await File.ReadAllTextAsync("openapi.yaml");
            text = text.Replace(oldValue: "PLUGIN_HOSTNAME", $"https://{host}");
            context.Response.ContentType = "text/yaml";
            await context.Response.WriteAsync(text);
        })
   .WithName("OpenApiSpec")
   .WithOpenApi();

app.Run();

internal record TodoItem(string Todo);

internal static class HttpContextExtensions
{
    public static TValue GetOrCreate<TValue>(this IDictionary<object, object?> items, object key) where TValue : new()
    {
        if (items.TryGetValue(key, out object? value)) return (TValue)value!;

        value = new TValue();
        items[key] = value;

        return (TValue)value;
    }
}
