using System.Text.Json.Serialization;

namespace Toto.ApiGateway.Models;

public class ClaimsDto
{
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }

    public ClaimsDto()
    {
    }

    public ClaimsDto(Guid userId)
    {
        UserId = userId;
    }
}