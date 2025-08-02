namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Utilities;

public static partial class DomainErrors
{
    public static class FirstName
    {
        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            DisplayNameHelper.ToSentence(nameof(FirstName)));

        public static readonly Error TooLong = Shared.Errors.DomainErrors.TooLong(
            DisplayNameHelper.ToSentence(nameof(FirstName)),
            ValueObjects.FullName.MaxLength);
    }

    public static class LastName
    {
        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            DisplayNameHelper.ToSentence(nameof(LastName)));

        public static readonly Error TooLong = Shared.Errors.DomainErrors.TooLong(
            DisplayNameHelper.ToSentence(nameof(LastName)),
            ValueObjects.FullName.MaxLength);
    }
}
