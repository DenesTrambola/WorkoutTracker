namespace WorkoutTracker.Domain.Users.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public sealed record WorkoutId : StronglyTypedId<Guid>
{
    private WorkoutId(Guid id)
        : base(id)
    {
    }

    private WorkoutId()
        : base(Guid.Empty)
    {
    }

    public static Result<WorkoutId> New() // Consider renaming to CreateNew
    {
        return new WorkoutId(Guid.NewGuid());
    }

    public static Result<WorkoutId> FromGuid(Guid value)
    {
        return Result.Ensure(
            value,
            v => v != Guid.Empty,
            DomainErrors.WorkoutId.Empty)
            .Map(v => new WorkoutId(v));
    }

    public static Result<WorkoutId> EnsureNotNull(WorkoutId routineId)
    {
        return Result.Ensure(
            routineId,
            woId => woId is not null,
            DomainErrors.WorkoutId.Null);
    }
}
