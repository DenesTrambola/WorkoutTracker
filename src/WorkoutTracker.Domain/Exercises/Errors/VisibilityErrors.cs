namespace WorkoutTracker.Domain.Exercises.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Visibility
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(Visibility));
    }
}
