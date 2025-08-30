namespace WorkoutTracker.Persistence.Repositories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<User>> AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Users.AddAsync(entity, cancellationToken);
            return Result.Success(entity);
        }
        catch (Exception)
        {
            return Result.Failure<User>(ApplicationErrors.User.CannotAddToDatabase);
        }
    }

    public async Task<Result> DeleteAsync(UserId id, CancellationToken cancellationToken = default)
    {
        var userResult = await GetByIdAsync(id, cancellationToken);

        try
        {
            _dbContext.Users.Remove(userResult.ValueOrDefault()!);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.User.CannotDeleteFromDatabase);
        }
    }

    public async Task<Result<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _dbContext.Users.ToListAsync(cancellationToken);
        return Result.Success(users.AsEnumerable());
    }

    public async Task<Result<User>> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken),
            u => u is not null,
            ApplicationErrors.User.NotFound)!;
    }

    public async Task<Result<User>> GetByUsernameAsync(Username username, CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken),
            u => u is not null,
            ApplicationErrors.User.NotFound)!;
    }

    public async Task<Result<Email>> ValidateEmailUniqueness(Email email, CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            !(await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken)),
            ApplicationErrors.Email.Taken)
            .OnSuccess(() => email);
    }

    public async Task<Result<Username>> ValidateUsernameUniqueness(Username username, CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            !(await _dbContext.Users.AnyAsync(u => u.Username == username, cancellationToken)),
            ApplicationErrors.Username.Taken)
            .OnSuccess(() => username);
    }
}
