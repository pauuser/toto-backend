using System.Threading.Tasks;
using Toto.AuthService.Domain.Enums;
using Toto.AuthService.Domain.Models;

namespace Toto.AuthService.Services.External;

public class GoogleAuthProcessorStrategy : IExternalAuthProcessorStrategy
{
    public AuthProvider Provider => AuthProvider.Google;

    public async Task<UserData> Authenticate(string code)
    {
        return new UserData
        {
            Email = "toto-google@gmail.com",
            FirstName = "Kirill",
            LastName = "Ivanov",
        };
    }
}