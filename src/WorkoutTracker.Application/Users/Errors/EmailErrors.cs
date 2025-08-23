namespace WorkoutTracker.Application.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class Email
    {
        public static readonly Error SendingFailed = new(
            $"{nameof(Email)}.SendingFailed",
            $"An error occurred while sending the {nameof(Email)}.");

        public static readonly Error Taken = Shared.Errors.ApplicationErrors.Taken(
            nameof(Email));
    }
}
