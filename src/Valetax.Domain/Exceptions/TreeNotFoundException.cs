namespace Valetax.Domain.Exceptions;

public sealed class TreeNotFoundException : SecureException
{
    public TreeNotFoundException(string treeName)
        : base($"Tree with name '{treeName}' was not found")
    {
    }
}
