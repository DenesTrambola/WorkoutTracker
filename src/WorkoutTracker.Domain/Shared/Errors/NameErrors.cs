namespace WorkoutTracker.Domain.Shared.Errors;
public static partial class DomainErrors
{
    public static class Name
    {
        public static readonly Error Empty = DomainErrors.Empty(
            nameof(Name));

        public static readonly Error TooLong = TooLong(
            nameof(Name),
            ValueObjects.Name.MaxLength);
    }
}
