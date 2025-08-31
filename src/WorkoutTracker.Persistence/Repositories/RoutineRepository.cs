namespace WorkoutTracker.Persistence.Repositories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Application.Routines.Errors;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Results;

public sealed class RoutineRepository(
    AppDbContext dbContext)
    : IRoutineRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Result<Routine>> AddAsync(
        Routine entity,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Routines.AddAsync(entity, cancellationToken);
            return Result.Success(entity);
        }
        catch (Exception)
        {
            return Result.Failure<Routine>(ApplicationErrors.Routine.CannotAddToDatabase);
        }
    }

    public async Task<Result> DeleteAsync(
        RoutineId id,
        CancellationToken cancellationToken = default)
    {
        var routineResult = await GetByIdAsync(id, cancellationToken);

        if (routineResult.IsFailure)
            return Result.Failure(routineResult.Errors);

        try
        {
            _dbContext.Routines.Remove(routineResult.ValueOrDefault()!);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Routine.CannotDeleteFromDatabase);
        }
    }

    public async Task<Result<IEnumerable<Routine>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var routines = await _dbContext.Routines.ToListAsync(cancellationToken);

        return Result.Success(routines.AsEnumerable());
    }

    public async Task<Result<Routine>> GetByIdAsync(
        RoutineId id,
        CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            await _dbContext.Routines.FirstOrDefaultAsync(r => r.Id == id, cancellationToken),
            r => r is not null,
            ApplicationErrors.Routine.NotFound)!;
    }
}
