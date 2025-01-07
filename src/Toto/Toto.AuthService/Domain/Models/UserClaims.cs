using System;

namespace Toto.AuthService.Domain.Models;

public record UserClaims
{
    public const string UserIdClaimName = "userId";
    public required Guid UserId { get; init; }
}