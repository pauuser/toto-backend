using Toto.AuthService.Domain.Models;

namespace Toto.AuthService.DataAccess.Models.Converters;

public static class TokensConverter
{
    public static Tokens? ToDomain(this TokensDb? tokens)
    {
        if (tokens is null)
            return null;
        
        return new Tokens
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            CreatedAtUtc = tokens.CreatedAtUtc,
            UserId = tokens.UserId
        };
    }
    
    public static TokensDb ToDb(this Tokens tokens)
    {
        return new TokensDb(id: Guid.NewGuid(),
            accessToken: tokens.AccessToken, 
            refreshToken: tokens.RefreshToken, 
            createdAtUtc: tokens.CreatedAtUtc,
            userId: tokens.UserId);
    }
}