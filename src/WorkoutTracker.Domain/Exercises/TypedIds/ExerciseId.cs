namespace WorkoutTracker.Domain.Exercises.TypedIds;

using WorkoutTracker.Domain.Exercises.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed record ExerciseId : StronglyTypedId<Guid>
{
    private ExerciseId(Guid id)
        : base(id)
    {
    }

    private ExerciseId()
        : base(Guid.Empty)
    {
    }

    public static Result<ExerciseId> New() // Consider renaming to CreateNew
    {
        return new ExerciseId(Guid.NewGuid());
    }

    public static Result<ExerciseId> FromGuid(Guid value)
    {
        return Result.Ensure(
            value,
            v => v != Guid.Empty,
            DomainErrors.ExerciseId.Empty)
            .Map(v => new ExerciseId(v));
    }

    public static Result<ExerciseId> EnsureNotNull(ExerciseId exerciseId)
    {
        return Result.Ensure(
            exerciseId,
            eId => eId is not null,
            DomainErrors.ExerciseId.Null);
    }
}
