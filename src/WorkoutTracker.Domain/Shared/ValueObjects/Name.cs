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
        => Value = value;

    public static Result<Name> Create(string value)
        => Result.Combine(
            EmptyCheck(value),
            LengthCheck(value))
        .Map(v => new Name(v));

    private static Result<string> EmptyCheck(string value)
        => Result.Ensure(
            value,
            value => !string.IsNullOrWhiteSpace(value),
            DomainErrors.Name.Empty);

    private static Result<string> LengthCheck(string value)
        => Result.Ensure(
            value,
            value => MaxLength > value.Length,
            DomainErrors.Name.TooLong);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
