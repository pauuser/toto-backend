using MassTransit;
using Toto.AuthService.Domain.Interfaces;
using Toto.Contracts;
using Toto.Extensions.DI;

namespace Toto.AuthService.Consumers;

public class ValidateTokenConsumer(ITokenService tokenService) : IConsumer<ValidateToken>
{
    private readonly ITokenService _tokenService = tokenService.ThrowIfNull();

    public async Task Consume(ConsumeContext<ValidateToken> context)
    {
        var claims = await _tokenService.ValidateTokenAndExtractClaimsAsync(context.Message.AccessToken);

        await context.RespondAsync<ValidateTokenResult>(new
        {
            UserId = claims.UserId,
        });
    }
}