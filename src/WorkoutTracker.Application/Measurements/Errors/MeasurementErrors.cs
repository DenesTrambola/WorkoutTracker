namespace WorkoutTracker.Application.Measurements.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class Measurement
    {
        public static readonly Error CannotAddToDatabase
            = Shared.Errors.ApplicationErrors.CannotAddToDatabase(
            nameof(Measurement));

        public static readonly Error CannotDeleteFromDatabase
            = Shared.Errors.ApplicationErrors.CannotDeleteFromDatabase(
            nameof(Measurement));

        public static readonly Error NotFound = Shared.Errors.ApplicationErrors.NotFound(
            nameof(Measurement));
    }
}
