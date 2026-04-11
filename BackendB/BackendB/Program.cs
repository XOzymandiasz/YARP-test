var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/ping", () => Results.Ok("pong-B"));
app.MapGet("/api/hello", () => Results.Ok("hello from backend B"));
app.MapGet("/health", () => Results.Ok("healthy-B"));
app.MapGet("/api/slow", async () =>
{
    await Task.Delay(1500);
    return Results.Ok("slow-B");
});
app.MapGet("/api/large", () => Results.Text(new string('Y', 100000)));

app.Run("http://0.0.0.0:8080");