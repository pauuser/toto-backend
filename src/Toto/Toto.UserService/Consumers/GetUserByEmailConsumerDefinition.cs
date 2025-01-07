using MassTransit;

namespace Toto.UserService.Consumers
{
    public class GetUserByEmailConsumerDefinition : ConsumerDefinition<GetUserByEmailConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<GetUserByEmailConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));

            endpointConfigurator.UseInMemoryOutbox(context);
        }
    }
}