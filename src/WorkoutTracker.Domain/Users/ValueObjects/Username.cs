namespace WorkoutTracker.Domain.Users.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public class Username : ValueObject
{
    public const short MaxLength = 32;

    public string Value { get; private set; }

    private Username(string value)
    {
        Value = value;
    }

    public static Result<Username> Create(string value)
    {
        return Result.Combine(
            EnsureNotEmpty(value),
            EnsureNotTooLong(value))
            .Map(v => new Username(v));
    }

    private static Result<string> EnsureNotEmpty(string value)
    {
        return Result.Ensure(
            value,
            value => !string.IsNullOrWhiteSpace(value),
            DomainErrors.Username.Empty);
    }

    private static Result<string> EnsureNotTooLong(string value)
    {
        return Result.Ensure(
            value,
            value => value.Length <= MaxLength,
            DomainErrors.Username.TooLong);
    }

    public static Result<Username> EnsureNotNull(Username? username)
    {
        return username is not null
            ? Result.Success(username)
            : Result.Failure<Username>(DomainErrors.Username.Null);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
