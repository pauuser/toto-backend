using MassTransit;
using Toto.AuthService.Domain.Exceptions;
using Toto.AuthService.Domain.Interfaces;
using Toto.Contracts;
using Toto.Contracts.Models;
using Toto.Extensions.DI;

namespace Toto.AuthService.Consumers;

public class RefreshTokensConsumer(ILoginService loginService, ILogger<RefreshTokensConsumer> logger) : IConsumer<RefreshTokens>
{
    private readonly ILoginService _loginService = loginService.ThrowIfNull();
    private readonly ILogger<RefreshTokensConsumer> _logger = logger.ThrowIfNull();

    public async Task Consume(ConsumeContext<RefreshTokens> context)
    {
        try
        {
            var refreshToken = context.Message.RefreshToken;
            var newTokens = await _loginService.RefreshTokensAsync(refreshToken);

            await context.RespondAsync<RefreshTokenResult>(new
            {
                AccessToken = newTokens.AccessToken,
                RefreshToken = newTokens.RefreshToken,
            });
        }
        catch (TokensNotFoundException e)
        {
            _logger.LogInformation(e, "Refresh token not found");
            await context.RespondAsync<RefreshTokenResult>(new
            {
                IsSuccess = false,
                Error = ErrorContractDto.TokensNotFound
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error");
            await context.RespondAsync<LoginUserResult>(new
            {
                IsSuccess = false,
                Error = ErrorContractDto.InternalServerError,
            });
        }
    }
}