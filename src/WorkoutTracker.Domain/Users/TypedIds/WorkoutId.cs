namespace WorkoutTracker.Domain.Users.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

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
}
