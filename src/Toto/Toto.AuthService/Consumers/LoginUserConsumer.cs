using MassTransit;
using Toto.AuthService.Consumers.Converters;
using Toto.AuthService.Domain.Interfaces;
using Toto.Contracts;
using Toto.Contracts.Models;
using Toto.Extensions.DI;

namespace Toto.AuthService.Consumers;

public class LoginUserConsumer(ILogger<LoginUserConsumer> logger, ILoginService loginService) : IConsumer<LoginUser>
{
    private readonly ILoginService _loginService = loginService.ThrowIfNull();
    
    private readonly ILogger<LoginUserConsumer> _logger = logger.ThrowIfNull();

    public async Task Consume(ConsumeContext<LoginUser> context)
    {
        try
        {
            var loginData = context.Message;
            var tokens = await _loginService.ExternalLoginAsync(loginData.AuthProvider.ToDomain(), loginData.Code);

            await context.RespondAsync<LoginUserResult>(new
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
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