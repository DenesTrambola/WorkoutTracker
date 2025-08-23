namespace WorkoutTracker.Domain.Measurements.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class MeasurementId
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(MeasurementId));

        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            nameof(MeasurementId));
    }
}
