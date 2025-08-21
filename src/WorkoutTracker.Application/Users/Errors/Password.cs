namespace WorkoutTracker.Application.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class Password
    {
        public static readonly Error VerificationFailed = new Error(
            $"{nameof(Password)}.VerificationFailed",
            $"{nameof(Password)} verification failed.");
    }
}
