namespace WorkoutTracker.Domain.Measurements.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Measurement
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(Measurement));

        public static readonly Error NotFound = Shared.Errors.DomainErrors.NotFound(
            nameof(Measurement));
    }
}
