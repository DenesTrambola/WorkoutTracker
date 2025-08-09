namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Workout
    {
        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(Workout));

        public static readonly Error NotFound = Shared.Errors.DomainErrors.NotFound(
            nameof(Workout));

        public static readonly Error InvalidStartEndTime = new Error(
            $"{nameof(Workout)}.InvalidStartEndTime",
            "The start time must be earlier than the end time for a workout.");

        public static readonly Error InvalidRestTime = new Error(
            $"{nameof(Workout)}.InvalidRestTime",
            "The rest time between exercises must be a positive duration.");
    }
}
