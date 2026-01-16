namespace Valetax.Domain.Exceptions;

public sealed class DuplicateNodeNameException : SecureException
{
    public DuplicateNodeNameException(string nodeName)
        : base($"A node with name '{nodeName}' already exists at this level")
    {
    }
}
