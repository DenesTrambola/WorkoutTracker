namespace WorkoutTracker.Application.Users.Primitives;

using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.ValueObjects;

public interface IPasswordHasher
{
    Task<Result<PasswordHash>> HashAsync(Password password,
        CancellationToken cancellationToken = default);

    Task<Result<bool>> VerifyAsync(Password password, PasswordHash passwordHash);
}
