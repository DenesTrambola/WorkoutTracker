namespace WorkoutTracker.Domain.Shared.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class Description : ValueObject
{
    public string Value { get; private set; }

    private Description(string value) => Value = value;

    public static Result<Description> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Description>(DomainErrors.DescriptionErrors.NullOrWhiteSpace);

        return new Description(value);
    }

    public override IEnumerable<object> GetAtomicValues() => throw new NotImplementedException();
}
