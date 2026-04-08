var app = WebApplication.Create();

app.MapGet("/api/ping", () => "pong-A");
app.MapGet("/api/slow", async () =>
{
    await Task.Delay(2000);
    return "slow-A";
});
app.MapGet("/api/large", () => new string('X', 100000));

app.Run("http://localhost:5001");