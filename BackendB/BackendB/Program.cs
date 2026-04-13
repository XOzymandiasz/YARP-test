using System.IO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/test", (HttpRequest request) =>
{
    var header = request.Headers["X-Test"].ToString();

    return Results.Json(new HeaderResponse
    {
        HeaderValue = header
    });
});

app.MapGet("/api/products", (HttpRequest request) =>
{
    var id = request.Query["id"].ToString();
    var sort = request.Query["sort"].ToString();

    return Results.Json(new ProductsResponse
    {
        Id = id,
        Sort = sort
    });
});

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

public class HeaderResponse
{
    public string? HeaderValue { get; set; }
}

public class ProductsResponse
{
    public string? Id { get; set; }
    public string? Sort { get; set; }
}