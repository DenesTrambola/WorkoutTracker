namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Email
    {
        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            nameof(Email));

        public static readonly Error TooLong = Shared.Errors.DomainErrors.TooLong(
            nameof(Email),
            ValueObjects.Email.MaxLength);

        public static readonly Error InvalidFormat = Shared.Errors.DomainErrors.InvalidFormat(
            nameof(Email));
    }
}
