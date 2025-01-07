using Microsoft.EntityFrameworkCore;
using Toto.AuthService.DataAccess.Context;
using Toto.AuthService.DataAccess.Models;
using Toto.AuthService.DataAccess.Models.Converters;
using Toto.AuthService.Domain.Interfaces;
using Toto.AuthService.Domain.Models;
using Toto.Extensions.DI;

namespace Toto.AuthService.DataAccess.Repositories;

public class TokenRepository(AuthDbContext authDbContext, ILogger<TokenRepository> logger) : ITokenRepository
{
    private readonly AuthDbContext _authDbContext = authDbContext.ThrowIfNull();
    private readonly ILogger<TokenRepository> _logger = logger.ThrowIfNull();

    public async Task<Tokens?> FindTokenPairByAccessTokenAsync(string accessToken)
    {
        var tokenPair = await _authDbContext.Tokens
            .FirstOrDefaultAsync(t => t.AccessToken == accessToken);

        return tokenPair.ToDomain();
    }

    public async Task<Tokens?> FindTokenPairByRefreshTokenAsync(string refreshToken)
    {
        var tokenPair = await _authDbContext.Tokens
            .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);

        return tokenPair.ToDomain();
    }

    public async Task<Tokens> AddTokenPairAsync(Tokens tokenPair)
    {
        var tokens = tokenPair.ToDb();
        await _authDbContext.Tokens.AddAsync(tokens);
        await _authDbContext.SaveChangesAsync();

        return tokens.ToDomain()!;
    }

    public async Task UpdateTokenPairAsync(Tokens oldTokenPair, Tokens newTokenPair)
    {
        var transaction = await _authDbContext.Database.BeginTransactionAsync();
        try
        {
            await _authDbContext.Tokens
                .Where(t => t.AccessToken == oldTokenPair.AccessToken && t.RefreshToken == oldTokenPair.RefreshToken)
                .ExecuteDeleteAsync();

            await _authDbContext.Tokens.AddAsync(newTokenPair.ToDb());
            await _authDbContext.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
        }
    }

    public async Task DeleteTokensAsync(string accessToken)
    {
        await _authDbContext.Tokens
            .Where(t => t.AccessToken == accessToken)
            .ExecuteDeleteAsync();
    }
}