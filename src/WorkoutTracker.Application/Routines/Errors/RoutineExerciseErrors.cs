namespace WorkoutTracker.Application.Routines.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class RoutineExercise
    {
        public static readonly Error CannotAddToDatabase
            = Shared.Errors.ApplicationErrors.CannotAddToDatabase(
            nameof(RoutineExercise));

        public static readonly Error CannotDeleteFromDatabase
            = Shared.Errors.ApplicationErrors.CannotDeleteFromDatabase(
            nameof(RoutineExercise));

        public static readonly Error CannotUpdateInDatabase
            = Shared.Errors.ApplicationErrors.CannotUpdateInDatabase(
            nameof(RoutineExercise));

        public static readonly Error NotFound = Shared.Errors.ApplicationErrors.NotFound(
            nameof(RoutineExercise));
    }
}
