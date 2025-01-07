namespace Toto.AuthService.DataAccess.Models;

public class TokensDb
{
    public Guid Id { get; set; }
    
    public string AccessToken { get; set; }
    
    public string RefreshToken { get; set; }
    
    public Guid UserId { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }

    public TokensDb()
    {
    }

    public TokensDb(Guid id, 
        string accessToken, 
        string refreshToken, 
        Guid userId,
        DateTime createdAtUtc)
    {
        Id = id;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        CreatedAtUtc = createdAtUtc;
        UserId = userId;
    }
}