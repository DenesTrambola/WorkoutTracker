namespace WorkoutTracker.Infrastructure.Services;

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
            var hashed = BCrypt.Net.BCrypt.HashPassword(password.Value, WorkFactor);
            return await Task.Run(() => Result.Success(
                PasswordHash.Create(hashed).ValueOrDefault()),
                cancellationToken);
        }
        catch (Exception)
        {
            return await Task.Run(() => Result.Failure<PasswordHash>(
                ApplicationErrors.PasswordHash.CannotHash),
                cancellationToken);
        }
    }

    public async Task<Result> VerifyAsync(
        Password password,
        PasswordHash passwordHash,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var verified = BCrypt.Net.BCrypt.Verify(password.Value, passwordHash.Value);
            return await Task.Run(() => verified
                ? Result.Success()
                : Result.Failure(ApplicationErrors.Password.VerificationFailed),
                cancellationToken);
        }
        catch (Exception)
        {
            return await Task.Run(() => Result.Failure(
                ApplicationErrors.Password.VerificationFailed),
                cancellationToken);
        }
    }
}
