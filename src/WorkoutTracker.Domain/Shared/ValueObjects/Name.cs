namespace WorkoutTracker.Domain.Shared.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class Name : ValueObject
{
    public string Value { get; private set; }

    private Name(string value) => Value = value;

    public static Result<Name> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Name>(DomainErrors.NameErrors.NullOrWhiteSpace);

        return new Name(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
