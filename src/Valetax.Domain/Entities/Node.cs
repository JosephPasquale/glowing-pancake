using Ardalis.GuardClauses;
using Valetax.Domain.Common;
using Valetax.Domain.ValueObjects;

namespace Valetax.Domain.Entities;

public sealed class Node : Entity, IAuditableEntity
{
    public string Name { get; private set; } = null!;
    public long TreeId { get; private set; }
    public long? ParentId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }

    public Tree Tree { get; private set; } = null!;
    public Node? Parent { get; private set; }

    private readonly List<Node> _children = [];
    public IReadOnlyCollection<Node> Children => _children.AsReadOnly();

    private Node()
    {
    }

    public static Node Create(NodeName name, long treeId, long? parentId, DateTime createdAt)
    {
        Guard.Against.Null(name);
        Guard.Against.NegativeOrZero(treeId);

        return new Node
        {
            Name = name.Value,
            TreeId = treeId,
            ParentId = parentId,
            CreatedAt = createdAt
        };
    }

    public void Rename(NodeName newName, DateTime modifiedAt)
    {
        Guard.Against.Null(newName);

        Name = newName.Value;
        ModifiedAt = modifiedAt;
    }

    public bool IsRoot => ParentId is null;

    public bool HasChildren => _children.Count > 0;
}
