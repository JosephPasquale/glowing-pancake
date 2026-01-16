using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace Valetax.API.Tests.Middleware;

public class ExceptionHandlingMiddlewareTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ExceptionHandlingMiddlewareTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task SecureException_ShouldReturn400WithMessage()
    {
        var token = await GetAuthToken();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsync(
            "/api.user.tree.node.delete?nodeId=999999",
            null);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        content.Should().NotBeNull();
        content!.Type.Should().NotBe("Exception");
        content.Data.Message.Should().Contain("999999");
    }

    [Fact]
    public async Task ValidationError_ShouldReturn400()
    {
        var token = await GetAuthToken();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsync(
            "/api.user.tree.node.create?treeName=&nodeName=Test",
            null);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private async Task<string> GetAuthToken()
    {
        var response = await _client.PostAsync(
            "/api.user.partner.rememberMe?code=test-user",
            null);
        var content = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return content!.Token;
    }

    private record TokenResponse(string Token);
    private record ErrorResponse(string Type, string Id, ErrorData Data);
    private record ErrorData(string Message);
}
