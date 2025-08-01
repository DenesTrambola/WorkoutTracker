namespace WorkoutTracker.Domain.Shared.Primitives;

public abstract record StronglyTypedId<TValue>(TValue Value);
