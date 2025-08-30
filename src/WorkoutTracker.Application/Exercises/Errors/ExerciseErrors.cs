namespace WorkoutTracker.Application.Exercises.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class Exercise
    {
        public static readonly Error CannotAddToDatabase
            = Shared.Errors.ApplicationErrors.CannotAddToDatabase(
            nameof(Exercise));

        public static readonly Error CannotDeleteFromDatabase
            = Shared.Errors.ApplicationErrors.CannotDeleteFromDatabase(
            nameof(Exercise));

        public static readonly Error NotFound = Shared.Errors.ApplicationErrors.NotFound(
            nameof(Exercise));
    }
}
