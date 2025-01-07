using System.Threading.Tasks;
using MassTransit;
using Toto.Contracts;
using Toto.Extensions;
using Toto.Extensions.DI;
using Toto.UserService.Domain.Interfaces;
using Toto.UserService.Domain.Models;

namespace Toto.UserService.Consumers;

public class GetUserByEmailConsumer(IUserService userService) : IConsumer<GetUserByEmail>
{
    private readonly IUserService _userService = userService.ThrowIfNull();

    public async Task Consume(ConsumeContext<GetUserByEmail> context)
    {
        var userData = context.Message;
        var user = await _userService.GetOrCreateUserByEmail(userData.Email, new UserData
        {
            FirstName = userData.FirstName,
            LastName = userData.LastName,
        });

        await context.RespondAsync<GetUserByEmailResult>(new
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
        });
    }
}