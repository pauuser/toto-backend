using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Toto.ApiGateway.Models;
using Toto.ApiGateway.Models.Converters;
using Toto.Contracts;
using Toto.Extensions;

namespace Toto.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IRequestClient<LoginUser> loginUserRequestClient) : ControllerBase
{
    private readonly IRequestClient<LoginUser> _loginUserRequestClient = loginUserRequestClient.ThrowIfNull();

    [HttpPost("login/{authProvider}")]
    public async Task<IActionResult> Login([FromRoute] AuthProviderDto authProvider, [FromQuery] string code)
    {
        var tokens = await _loginUserRequestClient.GetResponse<LoginUserResult>(new
        {
            Provider = authProvider.ToContract(),
            Code = code
        });
        
        return Ok(new TokensDto
        {
            AccessToken = tokens.Message.AccessToken,
            RefreshToken = tokens.Message.RefreshToken,
        });
    }
}