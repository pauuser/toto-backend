namespace Toto.Contracts;

public sealed class RefreshTokens
{
    public string RefreshToken { get; init; }
}

public sealed class RefreshTokenResult : RequestResult
{
    public string AccessToken { get; init; }
    
    public string RefreshToken { get; init; }
}