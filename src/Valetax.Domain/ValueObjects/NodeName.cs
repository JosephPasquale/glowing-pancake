using Ardalis.GuardClauses;
using Valetax.Domain.Common;

namespace Valetax.Domain.ValueObjects;

public sealed class NodeName : ValueObject
{
    public const int MaxLength = 100;

    public string Value { get; }

    private NodeName(string value)
    {
        Value = value;
    }

    public static NodeName Create(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value), "Node name cannot be empty");
        Guard.Against.StringTooLong(value, MaxLength, nameof(value), $"Node name cannot exceed {MaxLength} characters");

        return new NodeName(value.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(NodeName nodeName) => nodeName.Value;
}
