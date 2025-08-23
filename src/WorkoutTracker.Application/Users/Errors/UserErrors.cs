namespace WorkoutTracker.Application.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public partial class ApplicationErrors
{
    public static class User
    {
        public static readonly Error NotFound = Shared.Errors.ApplicationErrors.NotFound(
            nameof(User));

        public static readonly Error CannotAddToDatabase
            = Shared.Errors.ApplicationErrors.CannotAddToDatabase(
            nameof(User));

        public static readonly Error CannotDeleteFromDatabase
            = Shared.Errors.ApplicationErrors.CannotDeleteFromDatabase(
            nameof(User));
    }
}
