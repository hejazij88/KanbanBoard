using System.Net;
using System.Text;
using System.Text.Json;

namespace KanbanBoard.Tests.Unit.Helper;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Dictionary<string, HttpResponseMessage> _responses = new();
    private readonly Dictionary<string, Func<HttpRequestMessage, HttpResponseMessage>> _responseFunctions = new();

    public void AddResponse(string url, object response, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var json = JsonSerializer.Serialize(response);
        _responses[url] = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    public void AddResponseFunction(string url, Func<HttpRequestMessage, HttpResponseMessage> func)
    {
        _responseFunctions[url] = func;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var url = request.RequestUri?.ToString() ?? "";

        if (_responseFunctions.TryGetValue(url, out var func))
        {
            return Task.FromResult(func(request));
        }

        if (_responses.TryGetValue(url, out var response))
        {
            return Task.FromResult(response);
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
    }
}