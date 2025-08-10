namespace WorkoutTracker.Domain.Exercises.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Exercises.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class Visibility : ValueObject
{
    public bool IsPublic { get; private set; }

    private Visibility(bool isPublic)
    {
        IsPublic = isPublic;
    }

    public static Result<Visibility> Create(bool isPublic)
    {
        return new Visibility(isPublic);
    }

    public static Visibility Public()
    {
        return new Visibility(true);
    }

    public static Visibility Private()
    {
        return new Visibility(false);
    }

    public static Result<Visibility> EnsureNotNull(Visibility visibility)
    {
        return Result.Ensure(
            visibility,
            v => v is not null,
            DomainErrors.Visibility.Null);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return IsPublic;
    }
}
