using System.Threading.Tasks;
using Toto.AuthService.Domain.Enums;
using Toto.AuthService.Domain.Models;

namespace Toto.AuthService.Services.External;

public class AppleAuthProcessorStrategy : IExternalAuthProcessorStrategy
{
    public AuthProvider Provider => AuthProvider.Apple;
    
    public async Task<UserData> Authenticate(string code)
    {
        return new UserData
        {
            Email = "toto-apple@gmail.com",
            FirstName = "Pavel",
            LastName = null
        };
    }
}