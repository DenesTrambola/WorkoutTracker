namespace WorkoutTracker.Infrastructure;

using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Application.Users.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class PasswordHasherService : IPasswordHasher
{
    private const int WorkFactor = 12;

    public async Task<Result<PasswordHash>> HashAsync(
        Password password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            string hashed = BCrypt.Net.BCrypt.HashPassword(password.Value, WorkFactor);
            return await Task.FromResult(Result.Success(PasswordHash.Create(hashed).ValueOrDefault()));
        }
        catch (Exception)
        {
            return await Task.FromResult(Result.Failure<PasswordHash>(
                ApplicationErrors.PasswordHash.CannotHash));
        }
    }

    public async Task<Result> VerifyAsync(
        Password password,
        PasswordHash passwordHash,
        CancellationToken cancellationToken = default)
    {
        try
        {
            bool verified = BCrypt.Net.BCrypt.Verify(password.Value, passwordHash.Value);
            return await Task.FromResult(verified
                ? Result.Success()
                : Result.Failure(ApplicationErrors.Password.VerificationFailed));
        }
        catch (Exception)
        {
            return await Task.FromResult(Result.Failure(
                ApplicationErrors.Password.VerificationFailed));
        }
    }
}
