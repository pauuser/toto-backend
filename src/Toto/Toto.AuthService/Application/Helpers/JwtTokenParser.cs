using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Toto.AuthService.Domain.Exceptions;
using Toto.AuthService.Domain.Models;

namespace Toto.AuthService.Services.Helpers;

public static class JwtTokenParser
{
    public static List<Claim> ConvertUserClaimsToSecurityClaims(UserClaims claims)
    {
        return
        [
            new Claim(UserClaims.UserIdClaimName, claims.UserId.ToString()),
        ];
    }
    
    public static UserClaims ParseTokenClaims(JwtSecurityToken token)
    {
        var userId = ParseUserIdClaim(token);

        return new UserClaims
        {
            UserId = userId,
        };
    }
    
    private static Guid ParseUserIdClaim(JwtSecurityToken token)
    {
        var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == UserClaims.UserIdClaimName);
        if (userIdClaim is null)
            throw new ClaimNotFoundServiceException($"Required claim {UserClaims.UserIdClaimName} but could not find it in the token");

        return Guid.Parse(userIdClaim.Value);
    }
}