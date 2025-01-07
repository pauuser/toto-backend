using Toto.Contracts.Models;

namespace Toto.Contracts;

public sealed class LoginUser
{
    public string Code { get; init; }
    
    public AuthProviderContractDto AuthProvider { get; init; }
}

public sealed class LoginUserResult : RequestResult
{
    public string AccessToken { get; init; }
    
    public string RefreshToken { get; init; }
}