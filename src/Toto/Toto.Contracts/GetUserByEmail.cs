namespace Toto.Contracts;

public record GetUserByEmail
{
    public string Email { get; init; }
    
    public string? FirstName { get; init; }
    
    public string? LastName { get; init; }
}

public record GetUserByEmailResult
{
    public Guid Id { get; init; }
    
    public string Email { get; init; }
    
    public string FirstName { get; init; }
    
    public string LastName { get; init; }
}