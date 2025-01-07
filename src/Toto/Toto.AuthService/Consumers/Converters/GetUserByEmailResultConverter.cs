using Toto.AuthService.Domain.Models;
using Toto.Contracts;

namespace Toto.AuthService.Consumers.Converters;

public static class GetUserByEmailResultConverter
{
    public static User ToDomain(this GetUserByEmailResult getUserByEmailResult)
    {
        return new User
        {
            Id = getUserByEmailResult.Id,
            Email = getUserByEmailResult.Email,
        };
    }
}