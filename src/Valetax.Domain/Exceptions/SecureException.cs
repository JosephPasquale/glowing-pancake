namespace Valetax.Domain.Exceptions;

public class SecureException : DomainException
{
    public SecureException()
    {
    }

    public SecureException(string message)
        : base(message)
    {
    }

    public SecureException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
