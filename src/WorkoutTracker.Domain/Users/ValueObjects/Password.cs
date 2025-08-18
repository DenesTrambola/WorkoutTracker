namespace WorkoutTracker.Domain.Users.ValueObjects;

using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public class Password : ValueObject
{
    public const byte MinLength = 8;

    public string Value { get; private set; }

    private Password(string plainPassword)
    {
        Value = plainPassword;
    }

    public static Result<Password> Create(string plainPassword)
    {
        return Result.Combine(
            EnsureNotEmpty(plainPassword),
            EnsureLength(plainPassword),
            EnsureContainsUppercase(plainPassword),
            EnsureContainsLowercase(plainPassword),
            EnsureContainsDigit(plainPassword),
            EnsureContainsSpecial(plainPassword))
            .Map(pp => new Password(pp));
    }

    private static Result<string> EnsureNotEmpty(string plainPassword)
    {
        return Result.Ensure(
            plainPassword,
            pp => !string.IsNullOrWhiteSpace(pp),
            DomainErrors.Password.Empty);
    }

    private static Result<string> EnsureLength(string plainPassword) =>
        Result.Ensure(
            plainPassword,
            pp => pp.Length >= MinLength,
            DomainErrors.Password.TooShort);

    private static Result<string> EnsureContainsUppercase(string plainPassword) =>
        Result.Ensure(
            plainPassword,
            pp => pp.Any(char.IsUpper),
            DomainErrors.Password.MissingUppercase);

    private static Result<string> EnsureContainsLowercase(string plainPassword) =>
        Result.Ensure(
            plainPassword,
            pp => pp.Any(char.IsLower),
            DomainErrors.Password.MissingLowercase);

    private static Result<string> EnsureContainsDigit(string plainPassword) =>
        Result.Ensure(
            plainPassword,
            pp => pp.Any(char.IsDigit),
            DomainErrors.Password.MissingDigit);

    private static Result<string> EnsureContainsSpecial(string plainPassword) =>
        Result.Ensure(
            plainPassword,
            pp => pp.Any(ch => !char.IsLetterOrDigit(ch)),
            DomainErrors.Password.MissingSpecial);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
