using Ardalis.GuardClauses;
using Valetax.Domain.Common;

namespace Valetax.Domain.ValueObjects;

public sealed class TreeName : ValueObject
{
    public const int MaxLength = 100;

    public string Value { get; }

    private TreeName(string value)
    {
        Value = value;
    }

    public static TreeName Create(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value), "Tree name cannot be empty");
        Guard.Against.StringTooLong(value, MaxLength, nameof(value), $"Tree name cannot exceed {MaxLength} characters");

        return new TreeName(value.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(TreeName treeName) => treeName.Value;
}
