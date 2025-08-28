namespace WorkoutTracker.Domain.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Password
    {
        public static readonly Error Empty = Shared.Errors.DomainErrors.Empty(
            nameof(Password));

        public static readonly Error TooShort = new Error(
            $"{nameof(Password)}.TooShort",
            $"{nameof(Password)} must be at least {ValueObjects.Password.MinLength} characters long.");

        public static readonly Error MissingUppercase = new Error(
            $"{nameof(Password)}.MissingUppercase",
            $"{nameof(Password)} is missing uppercase characters.");

        public static readonly Error MissingLowercase = new Error(
            $"{nameof(Password)}.MissingLowercase",
            $"{nameof(Password)} is missing lowercase characters.");

        public static readonly Error MissingDigit = new Error(
            $"{nameof(Password)}.MissingDigit",
            $"{nameof(Password)} is missing digit characters.");

        public static readonly Error MissingSpecial = new Error(
            $"{nameof(Password)}.MissingSpecial",
            $"{nameof(Password)} is missing special characters.");
    }
}
