namespace WorkoutTracker.Domain.Users.ValueObjects;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public class Email : ValueObject
{
    public const short MaxLength = 320;

    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        return Result.Combine(
            EnsureNotEmpty(value),
            EnsureNotTooLong(value),
            EnsureFormatIsValid(value))
        .Map(v => new Email(v));
    }

    private static Result<string> EnsureNotEmpty(string value)
    {
        return Result.Ensure(
            value,
            value => !string.IsNullOrWhiteSpace(value),
            DomainErrors.Email.Empty);
    }

    private static Result<string> EnsureNotTooLong(string value)
    {
        return Result.Ensure(
            value,
            value => MaxLength >= value.Length,
            DomainErrors.Email.TooLong);
    }

    private static Result<string> EnsureFormatIsValid(string value)
    {
        return Result.Ensure(
            value,
            value => Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"),
            DomainErrors.Email.InvalidFormat);
    }

    public static Result<Email> EnsureNotNull(Email? email)
    {
        return email is not null
            ? Result.Success(email)
            : Result.Failure<Email>(DomainErrors.Email.Null);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
