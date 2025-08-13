namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Gender
    {
        public static readonly Error Invalid = Shared.Errors.DomainErrors.InvalidValue(
            nameof(Gender));
    }
}
