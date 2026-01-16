namespace Valetax.Domain.Common;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(long id) : base(id)
    {
    }
}
