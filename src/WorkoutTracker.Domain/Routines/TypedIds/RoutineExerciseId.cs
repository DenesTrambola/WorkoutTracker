namespace WorkoutTracker.Domain.Routines.TypedIds;

using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public record RoutineExerciseId : StronglyTypedId<Guid>
{
    protected RoutineExerciseId(Guid id)
        : base(id)
    {
    }

    public static RoutineExerciseId New()
    {
        return new RoutineExerciseId(Guid.NewGuid());
    }

    public static Result<RoutineExerciseId> EnsureNotNull(RoutineExerciseId routineExerciseId)
    {
        return Result.Ensure(
            routineExerciseId,
            reId => reId is not null,
            DomainErrors.RoutineExercise.Null);
    }
}
