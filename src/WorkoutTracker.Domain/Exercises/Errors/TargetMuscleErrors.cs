namespace WorkoutTracker.Domain.Exercises.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class TargetMuscle
    {
        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            nameof(TargetMuscle));

        public static readonly Error TooLong = Shared.Errors.DomainErrors.TooLong(
            nameof(TargetMuscle),
            ValueObjects.TargetMuscle.MaxLength);

        public static readonly Error Null = Shared.Errors.DomainErrors.Null(
            nameof(TargetMuscle));
    }
}
