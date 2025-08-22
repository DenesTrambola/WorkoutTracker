namespace WorkoutTracker.Domain.Shared.Primitives;

using WorkoutTracker.Domain.Shared.Results;

public abstract record StronglyTypedId<TId>
{
    public TId IdValue { get; init; }

    protected StronglyTypedId(TId id)
    {
        IdValue = id;
    }
}
