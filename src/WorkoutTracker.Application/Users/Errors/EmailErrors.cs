namespace WorkoutTracker.Application.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class Email
    {
        public static readonly Error CannotSend = new(
            $"{nameof(Email)}.CannotSend",
            $"An error occurred while sending the {nameof(Email)}.");

        public static readonly Error Taken = Shared.Errors.ApplicationErrors.Taken(
            nameof(Email));
    }
}
