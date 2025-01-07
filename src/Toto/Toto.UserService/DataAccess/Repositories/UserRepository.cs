using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Toto.Extensions;
using Toto.Extensions.DI;
using Toto.UserService.Application.Helpers;
using Toto.UserService.DataAccess.Context;
using Toto.UserService.DataAccess.Models;
using Toto.UserService.DataAccess.Models.Converters;
using Toto.UserService.Domain.Interfaces;
using Toto.UserService.Domain.Models;

namespace Toto.UserService.DataAccess.Repositories;

public class UserRepository(UserDbContext userDbContext, ILogger<UserRepository> logger) : IUserRepository
{
    private readonly UserDbContext _userDbContext = userDbContext.ThrowIfNull();
    private readonly ILogger<UserRepository> _logger = logger.ThrowIfNull();

    public async Task<User> GetOrCreateUserByEmail(string email, UserData userData)
    {
        var user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
        {
            _logger.LogInformation("User with email {Email} not found", email);
            
            user = new UserDb(id: Guid.NewGuid(),
                firstName: userData.FirstName ?? NameHelper.CreateRandomFirstName(),
                lastName: userData.LastName ?? NameHelper.CreateRandomLastName(),
                email: email,
                registeredAtUtc: DateTime.UtcNow);
            await _userDbContext.Users.AddAsync(user);
            
            await _userDbContext.SaveChangesAsync();
            _logger.LogInformation("User with email {Email} added", email);
        }

        return user.ToDomain();
    }
}