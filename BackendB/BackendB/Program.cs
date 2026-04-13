using System.IO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/test", () => Results.Ok("test-B"));

app.MapPost("/api/test-orders", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var body = await reader.ReadToEndAsync();

    return Results.Content(body, "application/json");
});

app.MapGet("/api/error", () =>
{
    return Results.Problem(
        title: "Backend error",
        detail: "Test 500 from backend",
        statusCode: StatusCodes.Status500InternalServerError);
});

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