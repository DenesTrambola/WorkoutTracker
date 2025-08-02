namespace WorkoutTracker.Domain.Exercises.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;

public record ExerciseId(Guid Value) : StronglyTypedId<Guid>(Value);
