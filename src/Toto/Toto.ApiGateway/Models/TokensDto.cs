using System.Text.Json.Serialization;

namespace Toto.ApiGateway.Models;

public class TokensDto
{
    [JsonPropertyName("accessToken")]
    public required string AccessToken { get; set; }
    
    [JsonPropertyName("refreshToken")]
    public required string RefreshToken { get; set; }
}