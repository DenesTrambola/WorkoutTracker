namespace WorkoutTracker.Domain.Users;

using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public interface IUserRepository : IRepository<User, UserId>
{
    Task<Result<bool>> IsUsernameUnique(
        Username username,
        CancellationToken cancellationToken = default);

    Task<Result<bool>> IsEmailUnique(
        Email email,
        CancellationToken cancellationToken = default);
}
