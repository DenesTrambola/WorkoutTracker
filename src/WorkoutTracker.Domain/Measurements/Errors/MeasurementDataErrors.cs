namespace WorkoutTracker.Domain.Measurements.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class MeasurementData
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(MeasurementData));

        public static readonly Error InvalidDate = new Error(
            $"{nameof(MeasurementData)}.InvalidDate",
            "The date of the measurement data must be a valid date in the past or today.");

        public static readonly Error NotFound = Shared.Errors.DomainErrors.NotFound(
            nameof(MeasurementData));

        public static readonly Error CannotRemove = Shared.Errors.DomainErrors.CannotRemove(
            nameof(MeasurementData),
            nameof(Measurement));
    }
}
