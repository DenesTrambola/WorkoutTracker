namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class WorkoutId
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(WorkoutId));

        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            nameof(WorkoutId));
    }
}
