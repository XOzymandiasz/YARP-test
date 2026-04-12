using System.IO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/test", () => Results.Ok("test-A"));
app.MapPost("/api/test-orders", () => async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();

    return Results.Text(body, "application/json");
});
app.MapGet("/api/ping", () => Results.Ok("pong-A"));
app.MapGet("/api/hello", () => Results.Ok("hello from backend A"));
app.MapGet("/health", () => Results.Ok("healthy-A"));
app.MapGet("/api/slow", async () =>
{
    await Task.Delay(2000);
    return Results.Ok("slow-A");
});
app.MapGet("/api/large", () => Results.Text(new string('X', 100000)));
app.MapPost("/api/large", async context =>
{
    var buffer = new byte[1024 * 8];
    long totalRead = 0;
    int read;
    while ((read = await context.Request.Body.ReadAsync(buffer, 0, buffer.Length)) > 0)
    {
        totalRead += read;
    }

    await context.Response.WriteAsync(totalRead.ToString());
});
app.MapGet("/api/cookies", async context =>
{
    if (context.Request.Cookies.TryGetValue("test-cookie", out var cookies))
    {
        await context.Response.WriteAsync(cookies);
    }
    else
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("cookies not found");
    }
});
app.MapGet("/api/test-headers", async context =>
{
    var headers = new Dictionary<string, string>
    {
        { "X-Forwarded-For", context.Request.Headers["X-Forwarded-For"].ToString() },
        { "X-Forwarded-Proto", context.Request.Headers["X-Forwarded-Proto"].ToString() },
        { "X-Forwarded-Host", context.Request.Headers["X-Forwarded-Host"].ToString() }
    };
    await context.Response.WriteAsJsonAsync(headers);
});
app.Run("http://0.0.0.0:8080");