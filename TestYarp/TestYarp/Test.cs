using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace TestYarp;

public class Test
{
    private readonly HttpClient _client = new()
    {
        BaseAddress = new Uri("http://localhost:5000")
    };
    
    //F1
    [Fact]
    public async Task Get_Request_Should_Return_200_From_Backend()
    {
        var response = await _client.GetAsync("/a/api/test");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.NotNull(body);
        Assert.NotEmpty(body);
    }
    
    //F2
    public record TestOrder(int Id, string Name);

    [Fact]
    public async Task Post_Request_Should_Preserve_Body()
    {
        var payload = new TestOrder(1, "test-order");

        var response = await _client.PostAsJsonAsync("/b/api/test-orders", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadFromJsonAsync<TestOrder>();

        Assert.NotNull(responseBody);
        Assert.Equal(payload.Id, responseBody!.Id);
        Assert.Equal(payload.Name, responseBody.Name);
    }
}