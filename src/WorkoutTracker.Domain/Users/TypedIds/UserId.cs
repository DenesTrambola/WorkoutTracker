namespace WorkoutTracker.Domain.Users.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public sealed record UserId : StronglyTypedId<Guid>
{
    private UserId(Guid value)
        : base(value)
    {
    }

    private UserId()
        : base(Guid.Empty)
    {
    }

    public static Result<UserId> New() // Consider renaming to CreateNew
    {
       return new UserId(Guid.NewGuid());
    }

    public static Result<UserId> FromGuid(Guid value)
    {
        return Result.Ensure(
            value,
            v => v != Guid.Empty,
            DomainErrors.UserId.Null)
            .Map(v => new UserId(v));
    }

    public static Result<UserId> EnsureNotNull(UserId userId)
    {
        return Result.Ensure(
            userId,
            uId => uId is not null,
            DomainErrors.UserId.Null);
    }
}
