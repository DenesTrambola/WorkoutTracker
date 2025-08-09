namespace WorkoutTracker.Domain.Shared.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class Name : ValueObject
{
    public const short MaxLength = 50;

    public string Value { get; private set; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Create(string value)
    {
        return Result.Combine(
            EnsureNotEmpty(value),
            EnsureNotTooLong(value))
            .Map(v => new Name(v));
    }

    private static Result<string> EnsureNotEmpty(string value)
    {
        return Result.Ensure(
            value,
            value => !string.IsNullOrWhiteSpace(value),
            DomainErrors.Name.Empty);
    }

    private static Result<string> EnsureNotTooLong(string value)
    {
        return Result.Ensure(
            value,
            value => MaxLength > value.Length,
            DomainErrors.Name.TooLong);
    }

    public static Result<Name> EnsureNotNull(Name name)
    {
        return Result.Ensure(
            name,
            n => n is not null,
            DomainErrors.Name.Null);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
