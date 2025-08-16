namespace WorkoutTracker.Domain.Users.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public record WorkoutId : StronglyTypedId<Guid>
{
    protected WorkoutId(Guid id)
        : base(id)
    {
    }

    public static Result<WorkoutId> New()
    {
        return new WorkoutId(Guid.NewGuid());
    }

    public static Result<WorkoutId> EnsureNotNull(WorkoutId routineId)
    {
        return Result.Ensure(
            routineId,
            woId => woId is not null,
            DomainErrors.WorkoutId.Null);
    }
}
