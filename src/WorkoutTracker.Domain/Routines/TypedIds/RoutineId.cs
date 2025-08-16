namespace WorkoutTracker.Domain.Routines.TypedIds;

using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public record RoutineId : StronglyTypedId<Guid>
{
    protected RoutineId(Guid id)
        : base(id)
    {
    }

    public static Result<RoutineId> New()
    {
        return new RoutineId(Guid.NewGuid());
    }

    public static Result<RoutineId> EnsureNotNull(RoutineId routineId)
    {
        return Result.Ensure(
            routineId,
            rId => rId is not null,
            DomainErrors.RoutineId.Null);
    }
}
