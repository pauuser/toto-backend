using Toto.AuthService.Domain.Models;

namespace Toto.AuthService.Domain.Interfaces;

public interface ITokenService
{
    /// <summary>
    ///     Refresh token generation
    /// </summary>
    /// <returns>Refresh token</returns>
    string GenerateRefreshToken();

    /// <summary>
    ///     Access token generation
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns>Access token</returns>
    string GenerateAccessToken(Guid userId);
    
    /// <summary>
    ///     Validate access token
    /// </summary>
    /// <param name="accessToken">Access token</param>
    /// <returns>User claims</returns>
    Task<UserClaims> ValidateTokenAndExtractClaimsAsync(string accessToken);
}