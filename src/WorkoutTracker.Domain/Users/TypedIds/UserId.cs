namespace WorkoutTracker.Domain.Users.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public record UserId : StronglyTypedId<Guid>
{
    private UserId(Guid id)
        : base(id)
    {
    }

    public static Result<UserId> New()
    {
       return new UserId(Guid.NewGuid());
    }

    public static Result<UserId> EnsureNotNull(UserId userId)
    {
        return Result.Ensure(
            userId,
            uId => uId is not null,
            DomainErrors.UserId.Null);
    }
}
