namespace WorkoutTracker.Application.Routines.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class Routine
    {
        public static readonly Error CannotAddToDatabase
            = Shared.Errors.ApplicationErrors.CannotAddToDatabase(
            nameof(Routine));

        public static readonly Error CannotDeleteFromDatabase
            = Shared.Errors.ApplicationErrors.CannotDeleteFromDatabase(
            nameof(Routine));

        public static readonly Error CannotUpdateInDatabase
            = Shared.Errors.ApplicationErrors.CannotUpdateInDatabase(
            nameof(Routine));

        public static readonly Error NotFound = Shared.Errors.ApplicationErrors.NotFound(
            nameof(Routine));
    }
}
