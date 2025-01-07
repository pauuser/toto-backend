using Toto.Contracts.Models;

namespace Toto.ApiGateway.Models.Converters;

public static class AuthProviderConverter
{
    public static AuthProviderContractDto ToContract(this AuthProviderDto authProviderDto)
    {
        return authProviderDto switch
        {
            AuthProviderDto.Apple => AuthProviderContractDto.Apple,
            AuthProviderDto.Google => AuthProviderContractDto.Google,
            _ => throw new ArgumentOutOfRangeException(nameof(authProviderDto), authProviderDto, null)
        };
    }
}