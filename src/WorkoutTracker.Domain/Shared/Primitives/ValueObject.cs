namespace WorkoutTracker.Domain.Shared.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object?> GetAtomicValues();

    public static bool operator ==(ValueObject? left, ValueObject? right)
        => left is not null && left.Equals(right);

    public static bool operator !=(ValueObject? left, ValueObject? right)
        => !(left == right);

    public override bool Equals(object? obj)
        => obj is ValueObject other && ValuesAreEqual(other);

    public bool Equals(ValueObject? other)
        => other is not null && ValuesAreEqual(other);

    public override int GetHashCode()
        => GetAtomicValues()
            .Aggregate(default(int), HashCode.Combine);

    private bool ValuesAreEqual(ValueObject other)
        => GetAtomicValues()
            .SequenceEqual(other.GetAtomicValues());
}
