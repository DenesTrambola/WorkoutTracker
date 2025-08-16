namespace WorkoutTracker.Domain.Routines.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class  DomainErrors
{
    public static class RoutineExerciseId
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(RoutineExerciseId));
    }
}
