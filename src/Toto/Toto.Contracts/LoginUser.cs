using Toto.Contracts.Models;

namespace Toto.Contracts;

public record LoginUser
{
    public string Code { get; init; }
    
    public AuthProviderContractDto AuthProvider { get; init; }
}

public record LoginUserResult
{
    public string AccessToken { get; init; }
    
    public string RefreshToken { get; init; }
}