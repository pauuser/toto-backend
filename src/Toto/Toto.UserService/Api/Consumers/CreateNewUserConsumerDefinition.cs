using MassTransit;

namespace Toto.UserService.Api.Consumers
{
    public class CreateNewUserConsumerDefinition :
        ConsumerDefinition<CreateNewUserConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateNewUserConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));

            endpointConfigurator.UseInMemoryOutbox(context);
        }
    }
}