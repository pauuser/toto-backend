namespace Toto.AuthService.Domain.Exceptions;

public class TokensNotFoundException : Exception
{
    public TokensNotFoundException(string message) : base(message) { }
    
    public TokensNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}