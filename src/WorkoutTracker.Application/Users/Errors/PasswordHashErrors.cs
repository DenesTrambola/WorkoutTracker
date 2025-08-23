namespace WorkoutTracker.Application.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class  PasswordHash
    {
        public static readonly Error CannotHash = new Error(
            $"{nameof(PasswordHash)}.CannotHash",
            "Failed to hash the provided password.");
    }
}
