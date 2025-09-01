namespace WorkoutTracker.Application.Measurements.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class MeasurementData
    {
        public static readonly Error CannotAddToDatabase
            = Shared.Errors.ApplicationErrors.CannotAddToDatabase(
            nameof(MeasurementData));

        public static readonly Error CannotDeleteFromDatabase
            = Shared.Errors.ApplicationErrors.CannotDeleteFromDatabase(
            nameof(MeasurementData));

        public static readonly Error CannotUpdateInDatabase
            = Shared.Errors.ApplicationErrors.CannotUpdateInDatabase(
            nameof(MeasurementData));

        public static readonly Error NotFound = Shared.Errors.ApplicationErrors.NotFound(
            nameof(MeasurementData));
    }
}
