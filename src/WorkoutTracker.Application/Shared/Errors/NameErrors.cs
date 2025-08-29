namespace WorkoutTracker.Application.Shared.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class Name
    {
        public static readonly Error Taken = ApplicationErrors.Taken(
            nameof(Name));
    }
}
