namespace Bookify.Domain.Abstractions;

public abstract class Entity
{
    protected Entity(Guid id)
    {
        Id = Id;
    }

    public Guid Id { get; init; }
}