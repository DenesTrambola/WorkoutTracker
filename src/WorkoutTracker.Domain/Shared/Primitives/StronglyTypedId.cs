namespace WorkoutTracker.Domain.Shared.Primitives;

using WorkoutTracker.Domain.Shared.Results;

public abstract record StronglyTypedId<TId>
{
    public TId Id { get; protected set; }

    protected StronglyTypedId(TId id)
    {
        Id = id;
    }
}
