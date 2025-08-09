namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class FirstName
    {
        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            nameof(FirstName));

        public static readonly Error TooLong = Shared.Errors.DomainErrors.TooLong(
            nameof(FirstName),
            ValueObjects.FullName.MaxLength);
    }

    public static class LastName
    {
        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            nameof(LastName));

        public static readonly Error TooLong = Shared.Errors.DomainErrors.TooLong(
            nameof(LastName),
            ValueObjects.FullName.MaxLength);
    }

    public static class FullName
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(FullName));
    }
}
