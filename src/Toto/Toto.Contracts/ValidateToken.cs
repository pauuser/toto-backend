namespace Toto.Contracts;

public record ValidateToken
{
    public string AccessToken { get; init; }
}

public record ValidateTokenResult
{
    public Guid UserId { get; set; }
}