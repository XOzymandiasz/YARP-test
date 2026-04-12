using System.Formats.Asn1;
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
    
    //F05
    [Fact]
    public async Task Yarp_Adds_XForwarded_Headers_To_Backend_Request()
    {
        var response = await _client.GetAsync("/a/api/test-headers");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.False(string.IsNullOrEmpty(result["X-Forwarded-For"]), "X-Forwarded-For should not be empty");
        Assert.Equal("http", result["X-Forwarded-Proto"]);
        Assert.Equal("localhost:5000", result["X-Forwarded-Host"]);
    }
    
    //F06
    [Fact]
    public async Task Yarp_Routes_To_Selected_Addresses()
    {
        var response = await _client.GetAsync("/a/api/ping");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal("\"pong-A\"", responseString);
        
        var responseB = await _client.GetAsync("/b/api/ping");
        responseB.EnsureSuccessStatusCode();
        var responseStringB = await responseB.Content.ReadAsStringAsync();
        Assert.Equal("\"pong-B\"", responseStringB);
    }
    
    //F08
    [Fact]
    public async Task Yarp_Returns_404_When_Route_Is_Not_Configured()
    {
        var response = await _client.GetAsync("/unknown-random-path");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    //F10
    [Fact]
    public async Task Yarp_Passes_Cookies_To_Backend_Successfully()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/a/api/cookies");
        request.Headers.Add("Cookie", "test-cookie=abc-123");
        var response = await _client.SendAsync(request);

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        Assert.Equal("abc-123", responseString);
        
    }
    
    //F12
    [Fact]
    public async Task Post_Request_Payload()
    {
        int payloadSize = 5 * 1024 * 1024;
        var largeData = new byte[payloadSize];
        new Random().NextBytes(largeData);
        var content = new ByteArrayContent(largeData);
        var response = await _client.PostAsync("/a/api/large", content);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal(payloadSize.ToString(), responseString);
    }
    
    //F13
    [Fact]
    public async Task Yarp_Handles_MultipleConcurrentRequests_Successfully()
    {
        const int RequestCount = 50;

        var tasks = Enumerable.Range(0, RequestCount).Select(_ => _client.GetAsync("/a/api/slow"));
        HttpResponseMessage[] responses = await Task.WhenAll(tasks);
        Assert.Equal(RequestCount, responses.Length);
        foreach (var response in responses)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
    
}