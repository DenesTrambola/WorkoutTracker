namespace WorkoutTracker.Domain.Users.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;

public record WorkoutId(Guid Value) : StronglyTypedId<Guid>(Value);
