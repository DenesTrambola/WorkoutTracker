namespace WorkoutTracker.Domain.Routines.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;

public record RoutineId(Guid Value) : StronglyTypedId<Guid>(Value);
