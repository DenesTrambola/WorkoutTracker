namespace WorkoutTracker.Domain.Exercises.TypedIds;

using WorkoutTracker.Domain.Exercises.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public record ExerciseId : StronglyTypedId<Guid>
{
    protected ExerciseId(Guid id)
        : base(id)
    {
    }

    public static Result<ExerciseId> New()
    {
        return new ExerciseId(Guid.NewGuid());
    }

    public static Result<ExerciseId> EnsureNotNull(ExerciseId exerciseId)
    {
        return Result.Ensure(
            exerciseId,
            eId => eId is not null,
            DomainErrors.ExerciseId.Null);
    }
}
