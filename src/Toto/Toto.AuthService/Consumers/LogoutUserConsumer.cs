using MassTransit;
using Toto.AuthService.Domain.Interfaces;
using Toto.Contracts;
using Toto.Extensions.DI;

namespace Toto.AuthService.Consumers;

public class LogoutUserConsumer(ILoginService loginService) : IConsumer<LogoutUser>
{
    private readonly ILoginService _loginService = loginService.ThrowIfNull();

    public async Task Consume(ConsumeContext<LogoutUser> context)
    {
        await _loginService.LogoutAsync(context.Message.AccessToken);
    }
}