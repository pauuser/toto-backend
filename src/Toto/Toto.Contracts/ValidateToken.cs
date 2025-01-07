namespace Toto.Contracts;

public sealed class ValidateToken
{
    public string AccessToken { get; init; }
}

public sealed class ValidateTokenResult : RequestResult
{
    public Guid UserId { get; set; }
}