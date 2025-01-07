using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Toto.Contracts;
using Toto.Extensions;

namespace Toto.AuthService.Consumers;

public class LoginUserConsumer(ILogger<LoginUserConsumer> logger) : IConsumer<LoginUser>
{
    private readonly ILogger<LoginUserConsumer> _logger = logger.ThrowIfNull();

    public async Task Consume(ConsumeContext<LoginUser> context)
    {
        _logger.LogInformation("Login user request received");

        await context.RespondAsync<LoginUserResult>(new
        {
            AccessToken = "aboba",
            RefreshToken = "asdfaf"
        });
    }
}