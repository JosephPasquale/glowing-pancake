using System.Text.Json.Serialization;

namespace Valetax.API.Models;

/// <summary>
/// Standard API error response format as per specification.
/// </summary>
public sealed record ApiErrorResponse(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("data")] ApiErrorData Data);

/// <summary>
/// Error data containing the message.
/// </summary>
public sealed record ApiErrorData(
    [property: JsonPropertyName("message")] string Message);
