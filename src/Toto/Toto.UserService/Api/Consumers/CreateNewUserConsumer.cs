using System.Threading.Tasks;
using MassTransit;
using Toto.Contracts;

namespace Toto.UserService.Api.Consumers
{
    public class CreateNewUserConsumer :
        IConsumer<CreateNewUser>
    {
        public Task Consume(ConsumeContext<CreateNewUser> context)
        {
            return Task.CompletedTask;
        }
    }
}