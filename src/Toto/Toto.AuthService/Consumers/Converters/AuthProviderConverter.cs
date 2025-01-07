using System;
using Toto.AuthService.Domain.Enums;
using Toto.Contracts.Models;

namespace Toto.AuthService.Consumers.Converters;

public static class AuthProviderConverter
{
    public static AuthProvider ToDomain(this AuthProviderContractDto provider)
    {
        return provider switch
        {
            AuthProviderContractDto.Apple => AuthProvider.Apple,
            AuthProviderContractDto.Google => AuthProvider.Google,
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };
    }
}