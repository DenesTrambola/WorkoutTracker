namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Username
    {
        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            nameof(Username));

        public static readonly Error TooLong = Shared.Errors.DomainErrors.TooLong(
            nameof(Username),
            ValueObjects.Username.MaxLength);
    }
}
