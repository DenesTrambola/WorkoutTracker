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
        => Muscle = muscle;

    public static Result<TargetMuscle> Create(string muscle)
        => Result.Combine(
            EmptyCheck(muscle),
            LengthCheck(muscle)
        ).Map(m => new TargetMuscle(m));

    private static Result<string> EmptyCheck(string muscle)
        => Result.Ensure(
            muscle,
            muscle => !string.IsNullOrWhiteSpace(muscle),
            DomainErrors.TargetMuscle.Empty);

    private static Result<string> LengthCheck(string muscle)
        => Result.Ensure(
            muscle,
            muscle => muscle.Length <= MaxLength,
            DomainErrors.TargetMuscle.TooLong);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Muscle;
    }
}
