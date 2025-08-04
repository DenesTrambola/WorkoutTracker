namespace WorkoutTracker.Domain.Routines.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Routine
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(Routine));
    }
}
