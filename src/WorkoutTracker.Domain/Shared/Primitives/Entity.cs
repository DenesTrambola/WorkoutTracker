namespace WorkoutTracker.Domain.Shared.Primitives;

public abstract class Entity<TId>
    : IEquatable<Entity<TId>>
{
    public TId Id { get; private init; }

    protected Entity()
    {
        Id = default!;
    }

    protected Entity(TId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id), "Id cannot be null.");
    }

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        => left is not null && left.Equals(right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        => !(left == right);

    public bool Equals(Entity<TId>? other)
        => other is not null && ReferenceEquals(this, other) && Id!.Equals(other.Id);

    public override bool Equals(object? obj)
        => obj is not null
        && obj.GetType() == GetType()
        && obj is Entity<TId> entity
        && Id!.Equals(entity.Id);

    public override int GetHashCode()
        => Id!.GetHashCode();
}
