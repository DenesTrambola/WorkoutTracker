namespace WorkoutTracker.Domain.Exercises.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Exercises.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class TargetMuscle : ValueObject
{
    public static readonly byte MaxLength = 50;

    public string Muscle { get; private set; }

    private TargetMuscle(string muscle)
    {
        Muscle = muscle;
    }

    public static Result<TargetMuscle> Create(string muscle)
    {
        return Result.Combine(
            EnsureNotEmpty(muscle),
            EnsureNotTooLong(muscle))
            .Map(m => new TargetMuscle(m));
    }

    private static Result<string> EnsureNotEmpty(string muscle)
    {
        return Result.Ensure(
            muscle,
            muscle => !string.IsNullOrWhiteSpace(muscle),
            DomainErrors.TargetMuscle.Empty);
    }

    private static Result<string> EnsureNotTooLong(string muscle)
    {
        return Result.Ensure(
            muscle,
            muscle => muscle.Length <= MaxLength,
            DomainErrors.TargetMuscle.TooLong);
    }

    public static Result<TargetMuscle> EnsureNotNull(TargetMuscle? targetMuscle)
    {
        return targetMuscle is not null
            ? Result.Success(targetMuscle)
            : Result.Failure<TargetMuscle>(DomainErrors.TargetMuscle.Null);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Muscle;
    }
}
