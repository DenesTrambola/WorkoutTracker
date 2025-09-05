namespace WorkoutTracker.Domain.Routines.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class ExercisePosition
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(ExercisePosition));

        public static Error Invalid => new(
            $"{nameof(RoutineExercise)}.InvalidPosition",
            "Position must be greater than or equal to 1.");

        public static Error DuplicatePositions => new(
            $"{nameof(RoutineExercise)}.DuplicatePositions",
            "Duplicate positions are not allowed.");
    }
}
