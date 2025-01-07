using Toto.AuthService.Domain.Models;

namespace Toto.AuthService.Domain.Interfaces;

public interface ITokenRepository
{
    Task<Tokens?> FindTokenPairByAccessTokenAsync(string accessToken);
    
    Task<Tokens?> FindTokenPairByRefreshTokenAsync(string refreshToken);
    
    Task<Tokens> AddTokenPairAsync(Tokens tokenPair);
    
    Task UpdateTokenPairAsync(Tokens oldTokenPair, Tokens newTokenPair);

    Task DeleteTokensAsync(string accessToken);
}