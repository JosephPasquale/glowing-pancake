namespace Valetax.Domain.Exceptions;

public sealed class InvalidNodeOperationException : SecureException
{
    public InvalidNodeOperationException(string message)
        : base(message)
    {
    }
}
