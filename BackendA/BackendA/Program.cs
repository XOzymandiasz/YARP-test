var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/ping", () => Results.Ok("pong-A"));
app.MapGet("/api/hello", () => Results.Ok("hello from backend A"));
app.MapGet("/health", () => Results.Ok("healthy-A"));
app.MapGet("/api/slow", async () =>
{
    await Task.Delay(2000);
    return Results.Ok("slow-A");
});
app.MapGet("/api/large", () => Results.Text(new string('X', 100000)));

app.Run("http://0.0.0.0:8080");