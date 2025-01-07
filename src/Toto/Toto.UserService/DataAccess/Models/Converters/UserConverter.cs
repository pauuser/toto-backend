using Toto.UserService.Domain.Models;

namespace Toto.UserService.DataAccess.Models.Converters;

public static class UserConverter
{
    public static User ToDomain(this UserDb user)
    {
        return new User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            RegisteredAtUtc = user.RegisteredAtUtc,
        };
    }
}