namespace WorkoutTracker.Domain.Exercises.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class RoutineExercise
    {
        public static Error InvalidPosition => new(
            $"{nameof(RoutineExercise)}.InvalidPosition",
            "Position must be greater than or equal to 1.");
    }
}
