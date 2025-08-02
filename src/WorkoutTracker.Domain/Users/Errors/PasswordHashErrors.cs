namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Utilities;

public static partial class DomainErrors
{
    public static class PasswordHash
    {
        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            DisplayNameHelper.ToSentence(nameof(PasswordHash)));
    }
}
