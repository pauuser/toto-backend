using System;

namespace Toto.UserService.Domain.Models;

public record User
{
    public required Guid Id { get; init; }
    
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
    
    public required string Email { get; init; }
    
    public required DateTime RegisteredAtUtc { get; init; }
}