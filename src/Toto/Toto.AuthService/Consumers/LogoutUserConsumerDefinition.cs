using MassTransit;

namespace Toto.AuthService.Consumers;

public class LogoutUserConsumerDefinition :
    ConsumerDefinition<LogoutUserConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<LogoutUserConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));

        endpointConfigurator.UseInMemoryOutbox(context);
    }
}