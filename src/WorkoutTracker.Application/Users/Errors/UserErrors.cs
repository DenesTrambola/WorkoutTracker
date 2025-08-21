namespace WorkoutTracker.Application.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public partial class ApplicationErrors
{
    public static class User
    {
        public static readonly Error NotFound = Shared.Errors.ApplicationErrors.NotFound(
            nameof(User));
    }
}
