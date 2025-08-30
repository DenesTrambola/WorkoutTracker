namespace WorkoutTracker.Domain.Users;

using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public interface IUserRepository
    : IRepository<User, UserId>
{
    Task<Result<Username>> ValidateUsernameUniqueness(
        Username username,
        CancellationToken cancellationToken = default);

    Task<Result<Email>> ValidateEmailUniqueness(
        Email email,
        CancellationToken cancellationToken = default);

    Task<Result<User>> GetByUsernameAsync(
        Username username,
        CancellationToken cancellationToken = default);
}
