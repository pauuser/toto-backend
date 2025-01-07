using MassTransit;

namespace Toto.AuthService.Consumers;

public class ValidateTokenConsumerDefinition :
    ConsumerDefinition<ValidateTokenConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ValidateTokenConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));

        endpointConfigurator.UseInMemoryOutbox(context);
    }
}