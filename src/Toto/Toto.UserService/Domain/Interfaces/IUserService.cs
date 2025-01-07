using System.Threading.Tasks;
using Toto.UserService.Domain.Models;

namespace Toto.UserService.Domain.Interfaces;

public interface IUserService
{
    Task<User> GetOrCreateUserByEmail(string email, UserData userData);
}