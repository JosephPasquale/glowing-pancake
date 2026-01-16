using Ardalis.GuardClauses;
using Valetax.Domain.Common;

namespace Valetax.Domain.Entities;

public sealed class User : AggregateRoot, IAuditableEntity
{
    public string UniqueCode { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private User()
    {
    }

    public static User Create(string uniqueCode, DateTime createdAt)
    {
        Guard.Against.NullOrWhiteSpace(uniqueCode);

        return new User
        {
            UniqueCode = uniqueCode,
            CreatedAt = createdAt
        };
    }

    public void RecordLogin(DateTime loginAt)
    {
        LastLoginAt = loginAt;
        ModifiedAt = loginAt;
    }
}
