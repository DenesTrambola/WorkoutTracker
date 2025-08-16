namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class User
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(User));
    }
}
