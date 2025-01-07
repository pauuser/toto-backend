using System.Threading.Tasks;
using Toto.AuthService.Domain.Enums;
using Toto.AuthService.Domain.Models;

namespace Toto.AuthService.Domain.Interfaces;

public interface ILoginService
{
    Task<Tokens> ExternalLoginAsync(AuthProvider provider, string authCode);

    Task LogoutAsync(string accessToken);

    Task<Tokens> RefreshTokensAsync(string refreshToken);
}