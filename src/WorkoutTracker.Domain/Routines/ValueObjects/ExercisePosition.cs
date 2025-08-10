namespace WorkoutTracker.Domain.Routines.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class ExercisePosition : ValueObject
{
    public byte Position { get; private set; }

    private ExercisePosition(byte position)
    {
        Position = position;
    }

    public static Result<ExercisePosition> Create(byte position)
    {
        return EnsureValueIsValid(position)
            .Map(p => new ExercisePosition(p));
    }

    public static Result<byte> EnsureValueIsValid(byte position)
    {
        return Result.Ensure(
            position,
            p => p >= 1,
            DomainErrors.RoutineExercise.InvalidPosition);
    }

    public static Result<ExercisePosition> EnsureNotNull(ExercisePosition position)
    {
        return Result.Ensure(
            position,
            p => p is not null,
            DomainErrors.ExercisePosition.Null);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Position;
    }
}
