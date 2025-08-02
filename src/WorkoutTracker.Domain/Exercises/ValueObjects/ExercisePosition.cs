namespace WorkoutTracker.Domain.Exercises.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Exercises.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class ExercisePosition : ValueObject
{
    public byte Position { get; private set; }

    private ExercisePosition(byte position)
        => Position = position;

    public static Result<ExercisePosition> Create(byte position)
        => Result.Ensure(
            position,
            position => position < 1,
            DomainErrors.RoutineExercise.InvalidPosition)
        .Map(p => new ExercisePosition(p));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Position;
    }
}
