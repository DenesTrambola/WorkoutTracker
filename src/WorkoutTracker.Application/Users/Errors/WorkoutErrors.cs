namespace WorkoutTracker.Application.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class Workout
    {
        public static readonly Error NotFound = Shared.Errors.ApplicationErrors.NotFound(
            nameof(Workout));

        public static readonly Error CannotAddToDatabase
            = Shared.Errors.ApplicationErrors.CannotAddToDatabase(
            nameof(Workout));

        public static readonly Error CannotUpdateInDatabase
            = Shared.Errors.ApplicationErrors.CannotUpdateInDatabase(
            nameof(Workout));

        public static readonly Error CannotDeleteFromDatabase
            = Shared.Errors.ApplicationErrors.CannotDeleteFromDatabase(
            nameof(Workout));
    }
}
