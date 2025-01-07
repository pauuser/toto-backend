using MassTransit;
using Toto.AuthService.Domain.Exceptions;
using Toto.AuthService.Domain.Interfaces;
using Toto.Contracts;
using Toto.Contracts.Models;
using Toto.Extensions.DI;

namespace Toto.AuthService.Consumers;

public class ValidateTokenConsumer(ITokenService tokenService) : IConsumer<ValidateToken>
{
    private readonly ITokenService _tokenService = tokenService.ThrowIfNull();

    public async Task Consume(ConsumeContext<ValidateToken> context)
    {
        try
        {
            var claims = await _tokenService.ValidateTokenAndExtractClaimsAsync(context.Message.AccessToken);

            await context.RespondAsync<ValidateTokenResult>(new
            {
                UserId = claims.UserId,
            });
        }
        catch (InvalidTokenException e)
        {
            await context.RespondAsync<ValidateTokenResult>(new
            {
                IsSuccess = false,
                Error = ErrorContractDto.InvalidToken
            });
        }
        catch (Exception e)
        {
            await context.RespondAsync<ValidateTokenResult>(new
            {
                IsSuccess = false,
                Error = ErrorContractDto.InternalServerError
            });
        }
    }
}