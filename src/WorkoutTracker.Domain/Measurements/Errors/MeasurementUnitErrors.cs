namespace WorkoutTracker.Domain.Measurements.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class MeasurementUnit
    {
        public static readonly Error Invalid = Shared.Errors.DomainErrors.InvalidValue(
            nameof(MeasurementUnit));
    }
}
