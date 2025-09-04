namespace WorkoutTracker.Domain.Routines.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class RoutineExercise
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(RoutineExercise));

        public static readonly Error InvalidSetCount = new Error(
            $"{nameof(RoutineExercise)}.InvalidSetCount",
            "The set count must be greater than zero.");

        public static readonly Error InvalidRepCount = new Error(
            $"{nameof(RoutineExercise)}.InvalidRepCount",
            "The rep count must be greater than zero.");

        public static readonly Error InvalidRestTimeBetweenSets = new Error(
            $"{nameof(RoutineExercise)}.InvalidRestTimeBetweenSets",
            "The rest time between sets must be greater than zero.");

        public static readonly Error CannotRemove = Shared.Errors.DomainErrors.CannotRemove(
            nameof(RoutineExercise),
            nameof(Routine));

        public static readonly Error NotFound = Shared.Errors.DomainErrors.NotFound(
            nameof(RoutineExercise));

        public static readonly Error ReorderIncomplete = new Error(
            $"{nameof(RoutineExercise)}.ReorderIncomplete",
            "An error occured while trying to reorder the exercises in the routine.");
    }
}
