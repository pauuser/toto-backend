using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Toto.AuthService.Domain.Configuration;
using Toto.AuthService.Domain.Exceptions;
using Toto.AuthService.Domain.Interfaces;
using Toto.AuthService.Domain.Models;
using Toto.AuthService.Services.Helpers;
using Toto.Extensions.DI;

namespace Toto.AuthService.Application;

public class TokenService(IOptions<JwtTokenConfiguration> jwtTokenConfiguration) : ITokenService
{
    private readonly JwtTokenConfiguration _jwtTokenConfiguration = jwtTokenConfiguration.Value.ThrowIfNull();

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }

    public string GenerateAccessToken(Guid userId)
    {
        var claims = JwtTokenParser.ConvertUserClaimsToSecurityClaims(new UserClaims
        {
            UserId = userId,
        });

        var keyBytes = Encoding.UTF8.GetBytes(_jwtTokenConfiguration.Key);
        var issuerSigningKey = new SymmetricSecurityKey(keyBytes);
        
        var jwt = new JwtSecurityToken(issuer: _jwtTokenConfiguration.Issuer,
            audience: _jwtTokenConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(_jwtTokenConfiguration.AccessLifetimeTimeSpan),
            signingCredentials: new SigningCredentials(key: issuerSigningKey,
                algorithm: SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public async Task<UserClaims> ValidateTokenAndExtractClaimsAsync(string accessToken)
    {
        try
        {
            var keyBytes = Encoding.UTF8.GetBytes(_jwtTokenConfiguration.Key);
            var issuerSigningKey = new SymmetricSecurityKey(keyBytes);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtTokenConfiguration.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtTokenConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = issuerSigningKey,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(_jwtTokenConfiguration.ClockSkewSeconds)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            accessToken = accessToken.Replace("Bearer ", string.Empty);
            var validationResult =
                await tokenHandler.ValidateTokenAsync(token: accessToken,
                    validationParameters: tokenValidationParameters);

            if (!validationResult.IsValid)
                throw validationResult.Exception;

            var jwtToken = (JwtSecurityToken)validationResult.SecurityToken;

            return JwtTokenParser.ParseTokenClaims(jwtToken);
        }
        catch (Exception e)
        {
            throw new InvalidTokenException("Invalid token", e);
        }
    }
}