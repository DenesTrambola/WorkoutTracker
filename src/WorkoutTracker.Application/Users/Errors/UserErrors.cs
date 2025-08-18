namespace WorkoutTracker.Application.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class User
    {
        public static readonly Error AddingToDatabaseFailed
            = Shared.Errors.ApplicationErrors.AddingToDatabaseFailed(
            nameof(User));
    }
}
