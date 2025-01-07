using MassTransit;

namespace Toto.AuthService.Consumers;

public class LoginUserConsumerDefinition :
    ConsumerDefinition<LoginUserConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<LoginUserConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));

        endpointConfigurator.UseInMemoryOutbox(context);
    }
}