namespace Toto.Extensions.PostgreSQL.Exceptions;

public class DatabaseNotFoundException : Exception
{
    public DatabaseNotFoundException() : base("Database not found")
    {
    }

    public DatabaseNotFoundException(string? message) : base(message)
    {
    }

    public DatabaseNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}