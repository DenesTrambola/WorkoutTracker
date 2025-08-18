namespace WorkoutTracker.Application.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class Email
    {
        public static readonly Error AlreadyExists = Shared.Errors.ApplicationErrors.AlreadyExists(
            nameof(Email));

        public static readonly Error SendingFailed = new Error(
            $"{nameof(Email)}.SendingFailed",
            $"An error occured while sending {nameof(Email)}.");
    }
}
