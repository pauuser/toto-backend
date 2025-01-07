using MassTransit;
using Toto.AuthService.Consumers.Converters;
using Toto.AuthService.Domain.Enums;
using Toto.AuthService.Domain.Exceptions;
using Toto.AuthService.Domain.Interfaces;
using Toto.AuthService.Domain.Models;
using Toto.AuthService.Services.External;
using Toto.Contracts;
using Toto.Extensions.DI;

namespace Toto.AuthService.Application;

public class LoginService(IRequestClient<GetUserByEmail> getUserByEmailRequestClient, 
    ITokenService tokenService, 
    ITokenRepository tokenRepository,
    ILogger<LoginService> logger) : ILoginService
{
    private readonly IRequestClient<GetUserByEmail> _getUserByEmailRequestClient = 
        getUserByEmailRequestClient.ThrowIfNull();
    
    private readonly ITokenService _tokenService = tokenService.ThrowIfNull();
    private readonly ITokenRepository _tokenRepository = tokenRepository.ThrowIfNull();

    private readonly ILogger<LoginService> _logger = logger.ThrowIfNull();
    
    private readonly List<IExternalAuthProcessorStrategy> _authStrategies =
    [
        new GoogleAuthProcessorStrategy(),
        new AppleAuthProcessorStrategy()
    ];

    public async Task<Tokens> ExternalLoginAsync(AuthProvider provider, string authCode)
    {
        var userData = await _authStrategies
            .First(s => s.Provider == provider)
            .AuthenticateAsync(authCode);
        var user = await _getUserByEmailRequestClient.GetResponse<GetUserByEmailResult>(new
        {
            Email = userData.Email,
            FirstName = userData.FirstName,
            LastName = userData.LastName
        });
        if (!user.Message.IsSuccess)
        {
            _logger.LogError("Failed to get user profile");
            throw new UserServiceUnavailableException("Failed to get user profile");
        }
        _logger.LogInformation("User {UserId} logged in successfully via {AuthProvider}", user.Message.Id, provider);
        
        var tokens = await _tokenRepository.AddTokenPairAsync(new Tokens
        {
            AccessToken = _tokenService.GenerateAccessToken(user.Message.Id),
            RefreshToken = _tokenService.GenerateRefreshToken(),
            CreatedAtUtc = DateTime.UtcNow,
            UserId = user.Message.Id
        });

        return tokens;
    }

    public async Task LogoutAsync(string accessToken)
    {
        await _tokenRepository.DeleteTokensAsync(accessToken);
        _logger.LogInformation("User with accessToken {AccessToken} logged out", accessToken);
    }

    public async Task<Tokens> RefreshTokensAsync(string refreshToken)
    {
        var oldTokenPair = await _tokenRepository.FindTokenPairByRefreshTokenAsync(refreshToken);
        if (oldTokenPair is null)
        {
            _logger.LogInformation("Tokens not found for refresh token {RefreshToken}", refreshToken);
            throw new TokensNotFoundException($"Tokens not found for refresh token {refreshToken}");
        }
        
        var newTokens = new Tokens
        {
            AccessToken = _tokenService.GenerateAccessToken(oldTokenPair.UserId),
            RefreshToken = _tokenService.GenerateRefreshToken(),
            CreatedAtUtc = DateTime.UtcNow,
            UserId = oldTokenPair.UserId
        };

        await _tokenRepository.UpdateTokenPairAsync(oldTokenPair: oldTokenPair, newTokenPair: newTokens);
        _logger.LogInformation("User with userId {UserId} refreshed tokens", oldTokenPair.UserId);

        return newTokens;
    }
}