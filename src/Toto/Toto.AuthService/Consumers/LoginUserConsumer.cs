using MassTransit;
using Toto.AuthService.Consumers.Converters;
using Toto.AuthService.Domain.Interfaces;
using Toto.Contracts;
using Toto.Extensions.DI;

namespace Toto.AuthService.Consumers;

public class LoginUserConsumer(ILogger<LoginUserConsumer> logger, ILoginService loginService) : IConsumer<LoginUser>
{
    private readonly ILogger<LoginUserConsumer> _logger = logger.ThrowIfNull();
    private readonly ILoginService _loginService = loginService.ThrowIfNull();

    public async Task Consume(ConsumeContext<LoginUser> context)
    {
        _logger.LogInformation("Login user request received");

        var loginData = context.Message;
        var tokens = await _loginService.ExternalLoginAsync(loginData.AuthProvider.ToDomain(), loginData.Code);

        await context.RespondAsync<LoginUserResult>(new
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        });
    }
}