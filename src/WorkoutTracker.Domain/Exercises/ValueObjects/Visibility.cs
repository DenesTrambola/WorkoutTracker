namespace WorkoutTracker.Domain.Exercises.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class Visibility : ValueObject
{
    public bool IsPublic { get; private set; }

    private Visibility(bool isPublic)
        => IsPublic = isPublic;

    public static Result<Visibility> Create(bool isPublic)
        => new Visibility(isPublic);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return IsPublic;
    }
}
