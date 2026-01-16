using Ardalis.GuardClauses;
using Valetax.Domain.Common;
using Valetax.Domain.Exceptions;
using Valetax.Domain.ValueObjects;

namespace Valetax.Domain.Entities;

public sealed class Tree : AggregateRoot, IAuditableEntity
{
    private readonly List<Node> _nodes = [];

    public string Name { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }

    public IReadOnlyCollection<Node> Nodes => _nodes.AsReadOnly();

    private Tree()
    {
    }

    public static Tree Create(TreeName name, DateTime createdAt)
    {
        Guard.Against.Null(name);

        return new Tree
        {
            Name = name.Value,
            CreatedAt = createdAt
        };
    }

    public Node AddNode(NodeName nodeName, long? parentNodeId, DateTime createdAt)
    {
        Guard.Against.Null(nodeName);

        ValidateNodeNameUniqueness(nodeName.Value, parentNodeId);

        var node = Node.Create(nodeName, Id, parentNodeId, createdAt);
        _nodes.Add(node);
        ModifiedAt = createdAt;

        return node;
    }

    public void RemoveNode(Node node, DateTime modifiedAt)
    {
        Guard.Against.Null(node);

        if (node.TreeId != Id)
            throw new InvalidNodeOperationException("Node does not belong to this tree");

        var hasChildren = _nodes.Any(n => n.ParentId == node.Id);
        if (hasChildren)
            throw new NodeHasChildrenException();

        _nodes.Remove(node);
        ModifiedAt = modifiedAt;
    }

    public void RenameNode(Node node, NodeName newName, DateTime modifiedAt)
    {
        Guard.Against.Null(node);
        Guard.Against.Null(newName);

        if (node.TreeId != Id)
            throw new InvalidNodeOperationException("Node does not belong to this tree");

        ValidateNodeNameUniqueness(newName.Value, node.ParentId, node.Id);

        node.Rename(newName, modifiedAt);
        ModifiedAt = modifiedAt;
    }

    private void ValidateNodeNameUniqueness(string nodeName, long? parentId, long? excludeNodeId = null)
    {
        var siblingExists = _nodes.Any(n =>
            n.ParentId == parentId &&
            n.Name.Equals(nodeName, StringComparison.OrdinalIgnoreCase) &&
            n.Id != excludeNodeId);

        if (siblingExists)
            throw new DuplicateNodeNameException(nodeName);
    }

    public void LoadNodes(IEnumerable<Node> nodes)
    {
        _nodes.Clear();
        _nodes.AddRange(nodes);
    }
}
