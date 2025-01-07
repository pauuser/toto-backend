using Toto.Extensions;
using Toto.Extensions.DI;
using Toto.UserService.Domain.Interfaces;
using Toto.UserService.Domain.Models;

namespace Toto.UserService.Application;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository.ThrowIfNull();

    public async Task<User> GetOrCreateUserByEmail(string email, UserData userData) => 
        await _userRepository.GetOrCreateUserByEmail(email, userData);
}