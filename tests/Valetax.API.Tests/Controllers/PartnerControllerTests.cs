using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace Valetax.API.Tests.Controllers;

public class PartnerControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PartnerControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RememberMe_WithValidCode_ShouldReturnToken()
    {
        var response = await _client.PostAsync(
            "/api.user.partner.rememberMe?code=test-unique-code",
            null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<TokenResponse>();
        content.Should().NotBeNull();
        content!.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task RememberMe_WithEmptyCode_ShouldReturnBadRequest()
    {
        var response = await _client.PostAsync(
            "/api.user.partner.rememberMe?code=",
            null);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private record TokenResponse(string Token);
}
