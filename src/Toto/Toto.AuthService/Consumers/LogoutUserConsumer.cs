using MassTransit;
using Toto.AuthService.Domain.Interfaces;
using Toto.Contracts;
using Toto.Extensions.DI;

namespace Toto.AuthService.Consumers;

public class LogoutUserConsumer(ILoginService loginService, ILogger<LogoutUserConsumer> logger) : IConsumer<LogoutUser>
{
    private readonly ILoginService _loginService = loginService.ThrowIfNull();
    
    private readonly ILogger<LogoutUserConsumer> _logger = logger.ThrowIfNull();

    public async Task Consume(ConsumeContext<LogoutUser> context)
    {
        try
        {
            await _loginService.LogoutAsync(context.Message.AccessToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error!");
            throw;
        }
    }
}