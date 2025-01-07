namespace Toto.Contracts;

public record RefreshTokens
{
    public string RefreshToken { get; init; }
}

public record RefreshTokenResult
{
    public string AccessToken { get; init; }
    
    public string RefreshToken { get; init; }
}