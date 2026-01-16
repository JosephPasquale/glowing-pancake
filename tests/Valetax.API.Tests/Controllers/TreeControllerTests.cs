using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace Valetax.API.Tests.Controllers;

public class TreeControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TreeControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTree_WithoutAuth_ShouldReturnUnauthorized()
    {
        var response = await _client.PostAsync(
            "/api.user.tree.get?treeName=TestTree",
            null);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetTree_WithAuth_NewTree_ShouldReturnNoContent()
    {
        var token = await GetAuthToken();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsync(
            "/api.user.tree.get?treeName=NewTestTree",
            null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task CreateNode_WithAuth_ShouldCreateNode()
    {
        var token = await GetAuthToken();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsync(
            "/api.user.tree.node.create?treeName=TestTree&nodeName=RootNode",
            null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<CreateNodeResponse>();
        content.Should().NotBeNull();
        content!.NodeId.Should().BeGreaterThan(0);
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
    private record CreateNodeResponse(long NodeId);
}
