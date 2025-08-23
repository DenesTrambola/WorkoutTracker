namespace WorkoutTracker.Domain.Routines.TypedIds;

using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed record RoutineExerciseId : StronglyTypedId<Guid>
{
    private RoutineExerciseId(Guid id)
        : base(id)
    {
    }

    private RoutineExerciseId()
        : base(Guid.Empty)
    {
    }

    public static RoutineExerciseId New() // Consider renaming to CreateNew
    {
        return new RoutineExerciseId(Guid.NewGuid());
    }

    public static Result<RoutineExerciseId> FromGuid(Guid value)
    {
        return Result.Ensure(
            value,
            v => v != Guid.Empty,
            DomainErrors.RoutineExerciseId.Empty)
            .Map(v => new RoutineExerciseId(v));
    }

    public static Result<RoutineExerciseId> EnsureNotNull(RoutineExerciseId routineExerciseId)
    {
        return Result.Ensure(
            routineExerciseId,
            reId => reId is not null,
            DomainErrors.RoutineExerciseId.Null);
    }
}
