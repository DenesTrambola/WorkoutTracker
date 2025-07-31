namespace WorkoutTracker.Domain.Shared.Primitives;

public abstract class Entity
    : IEquatable<Entity>
{
    public Guid Id { get; private init; }

    protected Entity(Guid id)
    {
        Id = id;
    }

    public static bool operator ==(Entity? left, Entity? right)
        => left is not null && left.Equals(right);

    public static bool operator !=(Entity? left, Entity? right)
        => !(left == right);

    public bool Equals(Entity? other)
        => other is not null && Id == other.Id;

    public override bool Equals(object? obj)
        => obj is not null && obj.GetType() == GetType() && obj is Entity entity && entity.Id == Id;

    public override int GetHashCode()
        => Id.GetHashCode();
}
