namespace Toto.UserService.Domain.Models;

public record UserData
{
    public required string? FirstName { get; init; }
    
    public required string? LastName { get; init; }
}