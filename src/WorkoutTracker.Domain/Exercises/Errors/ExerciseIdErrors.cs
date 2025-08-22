namespace WorkoutTracker.Domain.Exercises.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class ExerciseId
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(ExerciseId));

        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            nameof(ExerciseId));
    }
}
