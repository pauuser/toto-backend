namespace Toto.AuthService.Domain.Exceptions;

public class UserServiceUnavailableException : Exception
{
    public UserServiceUnavailableException(string message) : base(message) { }
    
    public UserServiceUnavailableException(string message, Exception innerException) : base(message, innerException) { }
}