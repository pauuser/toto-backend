using MassTransit.Futures.Contracts;

namespace Toto.AuthService.Domain.Models;

public record UserData
{
    public required string Email { get; set; }
    
    public required string? FirstName { get; set; }
    
    public required string? LastName { get; set; }
}