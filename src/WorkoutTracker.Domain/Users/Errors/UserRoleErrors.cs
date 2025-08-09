namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class UserRole
    {
        public static readonly Error InvalidValue = Shared.Errors.DomainErrors.InvalidValue(
            nameof(UserRole));
    }
}
