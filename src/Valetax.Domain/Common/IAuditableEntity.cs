namespace Valetax.Domain.Common;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; }
    DateTime? ModifiedAt { get; }
}
