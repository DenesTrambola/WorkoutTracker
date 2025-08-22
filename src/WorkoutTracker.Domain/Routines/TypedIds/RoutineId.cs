namespace WorkoutTracker.Domain.Routines.TypedIds;

using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed record RoutineId : StronglyTypedId<Guid>
{
    private RoutineId(Guid id)
        : base(id)
    {
    }

    private RoutineId()
        : base(Guid.Empty)
    {
    }

    public static Result<RoutineId> New() // Consider renaming to CreateNew
    {
        return new RoutineId(Guid.NewGuid());
    }

    public static Result<RoutineId> FromGuid(Guid value)
    {
        return Result.Ensure(
            value,
            v => v != Guid.Empty,
            DomainErrors.RoutineId.Empty)
            .Map(v => new RoutineId(v));
    }

    public static Result<RoutineId> EnsureNotNull(RoutineId routineId)
    {
        return Result.Ensure(
            routineId,
            rId => rId is not null,
            DomainErrors.RoutineId.Null);
    }
}
