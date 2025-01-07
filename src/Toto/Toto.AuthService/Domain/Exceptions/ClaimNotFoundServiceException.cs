namespace Toto.AuthService.Domain.Exceptions;

public class ClaimNotFoundServiceException : Exception
{
    public ClaimNotFoundServiceException(string message) : base(message) { }
    
    public ClaimNotFoundServiceException(string message, Exception innerException) : base(message, innerException) { }
}