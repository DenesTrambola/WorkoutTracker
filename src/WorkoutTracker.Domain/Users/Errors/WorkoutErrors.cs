namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Workout
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(Workout));
    }
}
