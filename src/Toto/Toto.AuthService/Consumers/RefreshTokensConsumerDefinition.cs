using MassTransit;

namespace Toto.AuthService.Consumers;

public class RefreshTokensConsumerDefinition :
    ConsumerDefinition<RefreshTokensConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<RefreshTokensConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseInMemoryOutbox(context);
    }
}