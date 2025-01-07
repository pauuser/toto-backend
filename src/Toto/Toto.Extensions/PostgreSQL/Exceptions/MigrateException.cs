namespace Toto.Extensions.PostgreSQL.Exceptions;

public class MigrateException : Exception
{
    public MigrateException(Exception? innerException) : base("Error while applying migrations", innerException)
    {
    }

    public MigrateException(string? message) : base(message)
    {
    }

    public MigrateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}