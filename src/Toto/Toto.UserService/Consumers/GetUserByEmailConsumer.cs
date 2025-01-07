using System.Threading.Tasks;
using MassTransit;
using Toto.Contracts;
using Toto.Contracts.Models;
using Toto.Extensions;
using Toto.Extensions.DI;
using Toto.UserService.Domain.Interfaces;
using Toto.UserService.Domain.Models;

namespace Toto.UserService.Consumers;

public class GetUserByEmailConsumer(IUserService userService, ILogger<GetUserByEmailConsumer> logger) : IConsumer<GetUserByEmail>
{
    private readonly IUserService _userService = userService.ThrowIfNull();
    private readonly ILogger<GetUserByEmailConsumer> _logger = logger.ThrowIfNull();

    public async Task Consume(ConsumeContext<GetUserByEmail> context)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error");
            await context.RespondAsync<GetUserByEmailResult>(new
            {
                IsSuccess = false,
                Error = ErrorContractDto.InternalServerError
            });
        }
    }
}