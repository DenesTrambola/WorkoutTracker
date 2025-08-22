namespace WorkoutTracker.Domain.Routines.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class ExercisePosition : ValueObject
{
    public byte Value { get; private set; }

    private ExercisePosition(byte position)
    {
        Value = position;
    }

    public static Result<ExercisePosition> Create(byte position)
    {
        return EnsureValueIsValid(position)
            .Map(p => new ExercisePosition(p));
    }

    private static Result<byte> EnsureValueIsValid(byte position)
    {
        return Result.Ensure(
            position,
            p => p > 0,
            DomainErrors.ExercisePosition.Invalid);
    }

    public static Result<ExercisePosition> EnsureNotNull(ExercisePosition? position)
    {
        return position is not null
            ? Result.Success(position)
            : Result.Failure<ExercisePosition>(DomainErrors.ExercisePosition.Null);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
