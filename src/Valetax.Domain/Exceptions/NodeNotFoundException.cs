namespace Valetax.Domain.Exceptions;

public sealed class NodeNotFoundException : SecureException
{
    public NodeNotFoundException(long nodeId)
        : base($"Node with ID = {nodeId} was not found")
    {
    }
}
