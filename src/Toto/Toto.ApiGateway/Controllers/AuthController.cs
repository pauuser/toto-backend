using System.Net.Http.Headers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Toto.ApiGateway.Models;
using Toto.ApiGateway.Models.Converters;
using Toto.Contracts;
using Toto.Contracts.Models;
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
        try
        {
            var provider = authProvider.ToContract();
            var tokens = await _loginUserRequestClient.GetResponse<LoginUserResult>(new
            {
                AuthProvider = provider,
                Code = code
            });
            if (!tokens.Message.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Login via AuthService failed");
            }
        
            return Ok(new TokensDto
            {
                AccessToken = tokens.Message.AccessToken,
                RefreshToken = tokens.Message.RefreshToken,
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    
    [HttpGet("claims")]
    public async Task<IActionResult> Validate()
    {
        try
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
            if (claims.Message is { IsSuccess: false, Error: ErrorContractDto.InvalidToken })
            {
                return Unauthorized("Token is invalid");
            }

            return Ok(new ClaimsDto(userId: claims.Message.UserId));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
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
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            var header = HttpContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(header))
                return BadRequest("Refresh token is required");

            var newTokenPair = await _refreshTokensRequestClient.GetResponse<RefreshTokenResult>(new
            {
                RefreshToken = header
            });
            if (newTokenPair.Message is { IsSuccess: false, Error: ErrorContractDto.TokensNotFound })
            {
                return Unauthorized("Token does not exist");
            }

            return Ok(new TokensDto
            {
                AccessToken = newTokenPair.Message.AccessToken,
                RefreshToken = newTokenPair.Message.RefreshToken,
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}