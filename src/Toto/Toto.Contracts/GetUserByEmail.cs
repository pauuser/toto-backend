namespace Toto.Contracts;

public sealed class GetUserByEmail
{
    public string Email { get; init; }
    
    public string? FirstName { get; init; }
    
    public string? LastName { get; init; }
}

public sealed class GetUserByEmailResult : RequestResult
{
    public Guid Id { get; init; }
    
    public string Email { get; init; }
    
    public string FirstName { get; init; }
    
    public string LastName { get; init; }
}