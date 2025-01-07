using System.Threading.Tasks;
using Toto.AuthService.Domain.Enums;
using Toto.AuthService.Domain.Models;

namespace Toto.AuthService.Services.External;

public interface IExternalAuthProcessorStrategy
{
    AuthProvider Provider { get; }
    
    Task<UserData> AuthenticateAsync(string code);
}