namespace WorkoutTracker.Domain.Measurements.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class MeasurementDataValue
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(MeasurementDataValue));

        public static Error Invalid => new(
            $"{nameof(MeasurementDataValue)}.InvalidValue",
            "Data value must be greater than or equal to 1.");
    }
}
