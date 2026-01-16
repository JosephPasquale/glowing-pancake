namespace Valetax.Domain.Exceptions;

public sealed class NodeHasChildrenException : SecureException
{
    public NodeHasChildrenException()
        : base("You have to delete all children nodes first")
    {
    }
}
