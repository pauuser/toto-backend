using System.ComponentModel.DataAnnotations;

namespace Toto.AuthService.Domain.Models;

/// <summary>
///     Tokens pair
/// </summary>
public record Tokens
{
    /// <summary>
    ///     Access token
    /// </summary>
    public required string AccessToken { get; init; }

    /// <summary>
    ///     Refresh token
    /// </summary>
    public required string RefreshToken { get; init; }
    
    /// <summary>
    ///     User identifier
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    ///     Time when token pair was created
    /// </summary>
    public required DateTime CreatedAtUtc { get; set; }
}