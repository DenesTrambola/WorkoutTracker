namespace WorkoutTracker.Domain.Users;

using System.Security.Cryptography;
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

    Task<Result<IEnumerable<Workout>>> GetAllWorkoutsAsync(
        CancellationToken cancellationToken = default);

    Task<Result<Workout>> GetWorkoutByIdAsync(
        WorkoutId id,
        CancellationToken cancellationToken = default);

    Task<Result<Workout>> AddWorkoutAsync(
        Workout workout,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteWorkoutAsync(
        WorkoutId id,
        CancellationToken cancellationToken = default);
}
