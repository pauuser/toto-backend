using System;

namespace Toto.AuthService.Domain.Models;

public record User
{
    public required Guid Id { get; init; }
    
    public required string Email { get; init; }
}