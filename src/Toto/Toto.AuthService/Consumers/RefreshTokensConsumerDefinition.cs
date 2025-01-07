using MassTransit;

namespace Toto.AuthService.Consumers;

public class RefreshTokensConsumerDefinition :
    ConsumerDefinition<RefreshTokensConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<RefreshTokensConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));

        endpointConfigurator.UseInMemoryOutbox(context);
    }
}