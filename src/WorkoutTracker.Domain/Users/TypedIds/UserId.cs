namespace WorkoutTracker.Domain.Users.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;

public record UserId(Guid value) : StronglyTypedId<Guid>(value);
