using MassTransit;
using Toto.AuthService.Domain.Interfaces;
using Toto.Contracts;
using Toto.Extensions.DI;

namespace Toto.AuthService.Consumers;

public class RefreshTokensConsumer(ILoginService loginService) : IConsumer<RefreshTokens>
{
    private readonly ILoginService _loginService = loginService.ThrowIfNull();

    public async Task Consume(ConsumeContext<RefreshTokens> context)
    {
        var refreshToken = context.Message.RefreshToken;
        var newTokens = await _loginService.RefreshTokensAsync(refreshToken);

        await context.RespondAsync<RefreshTokenResult>(new
        {
            AccessToken = newTokens.AccessToken,
            RefreshToken = newTokens.RefreshToken,
        });
    }
}