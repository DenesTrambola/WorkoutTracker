namespace WorkoutTracker.Domain.Routines.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;

public record RoutineExerciseId(Guid Value) : StronglyTypedId<Guid>(Value);
