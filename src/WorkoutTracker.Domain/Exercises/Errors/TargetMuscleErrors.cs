namespace WorkoutTracker.Domain.Exercises.Errors;

using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Utilities;

public static partial class DomainErrors
{
    public static class TargetMuscle
    {
        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            DisplayNameHelper.ToSentence(nameof(TargetMuscle)));

        public static readonly Error TooLong = Shared.Errors.DomainErrors.TooLong(
            DisplayNameHelper.ToSentence(nameof(TargetMuscle)),
            ValueObjects.TargetMuscle.MaxLength);
    }
}
