using MassTransit;

namespace Toto.AuthService.Consumers;

public class LogoutUserConsumerDefinition :
    ConsumerDefinition<LogoutUserConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<LogoutUserConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseInMemoryOutbox(context);
    }
}