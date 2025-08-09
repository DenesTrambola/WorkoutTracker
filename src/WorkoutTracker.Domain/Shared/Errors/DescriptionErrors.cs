namespace WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Description
    {
        public static readonly Error TooLong = TooLong(
            nameof(Description),
            ValueObjects.Description.MaxLength);

        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(Description));
    }
}
