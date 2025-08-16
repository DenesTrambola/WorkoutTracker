namespace WorkoutTracker.Domain.Exercises.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Exercise
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(Exercise));

        public static readonly Error NotFound = Shared.Errors.DomainErrors.NotFound(
            nameof(Exercise));
    }
}
