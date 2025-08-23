namespace WorkoutTracker.Application.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class Username
    {
        public static readonly Error Taken = Shared.Errors.ApplicationErrors.Taken(
            nameof(Username));
    }
}
