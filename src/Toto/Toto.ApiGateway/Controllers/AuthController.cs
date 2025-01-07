using System.Net.Http.Headers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Toto.ApiGateway.Models;
using Toto.ApiGateway.Models.Converters;
using Toto.Contracts;
using Toto.Extensions;
using Toto.Extensions.DI;

namespace Toto.ApiGateway.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IRequestClient<LoginUser> loginUserRequestClient, 
    ISendEndpointProvider sendEndpointProvider,
    IRequestClient<RefreshTokens> refreshTokensRequestClient,
    IRequestClient<ValidateToken> validateTokenRequestClient) : ControllerBase
{
    private readonly IRequestClient<LoginUser> _loginUserRequestClient = loginUserRequestClient.ThrowIfNull();
    private readonly IRequestClient<RefreshTokens> _refreshTokensRequestClient = refreshTokensRequestClient.ThrowIfNull();
    private readonly IRequestClient<ValidateToken> _validateTokenRequestClient = validateTokenRequestClient.ThrowIfNull();
    private readonly ISendEndpointProvider _sendEndpointProvider = sendEndpointProvider.ThrowIfNull();

    [HttpPost("login/{authProvider}")]
    public async Task<IActionResult> Login([FromRoute] AuthProviderDto authProvider, [FromQuery] string code)
    {
        var provider = authProvider.ToContract();
        var tokens = await _loginUserRequestClient.GetResponse<LoginUserResult>(new
        {
            AuthProvider = provider,
            Code = code
        });
        
        return Ok(new TokensDto
        {
            AccessToken = tokens.Message.AccessToken,
            RefreshToken = tokens.Message.RefreshToken,
        });
    }

    
    [HttpGet("claims")]
    public async Task<IActionResult> Validate()
    {
        var header = HttpContext.Request.Headers.Authorization;
        var isValidRequest = AuthenticationHeaderValue.TryParse(header, out var headerValue);

        if (!isValidRequest)
            return BadRequest("Access token is required");

        var token = headerValue!.Parameter ?? string.Empty;
        var claims = await _validateTokenRequestClient.GetResponse<ValidateTokenResult>(new
        {
            AccessToken = token,
        });

        return Ok(new ClaimsDto(userId: claims.Message.UserId));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var header = HttpContext.Request.Headers.Authorization;
        var isValidRequest = AuthenticationHeaderValue.TryParse(header, out var headerValue);

        if (!isValidRequest)
            return BadRequest("Access token is required");

        var token = headerValue!.Parameter ?? string.Empty;
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:logout-user"));
        await endpoint.Send(new LogoutUser
        {
            AccessToken = token
        });

        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var header = HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(header))
            return BadRequest("Refresh token is required");

        var newTokenPair = await _refreshTokensRequestClient.GetResponse<RefreshTokenResult>(new
        {
            RefreshToken = header
        });

        return Ok(new TokensDto
        {
            AccessToken = newTokenPair.Message.AccessToken,
            RefreshToken = newTokenPair.Message.RefreshToken,
        });
    }
}