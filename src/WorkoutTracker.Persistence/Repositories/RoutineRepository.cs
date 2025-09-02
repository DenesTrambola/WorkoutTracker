namespace WorkoutTracker.Persistence.Repositories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Application.Routines.Errors;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
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

    public async Task<Result<RoutineExercise>> AddExerciseAsync(
        RoutineExercise exercise,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.RoutineExercises.AddAsync(exercise, cancellationToken);
            return Result.Success(exercise);
        }
        catch (Exception)
        {
            return Result.Failure<RoutineExercise>(ApplicationErrors.RoutineExercise.CannotAddToDatabase);
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

    public async Task<Result> DeleteExerciseAsync(
        RoutineExerciseId exerciseId,
        CancellationToken cancellationToken = default)
    {
        var exerciseResult = await GetExerciseByIdAsync(exerciseId, cancellationToken);

        if (exerciseResult.IsFailure)
            return Result.Failure(exerciseResult.Errors);

        try
        {
            _dbContext.RoutineExercises.Remove(exerciseResult.ValueOrDefault()!);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.RoutineExercise.CannotDeleteFromDatabase);
        }
    }

    public async Task<Result<IEnumerable<Routine>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var routines = await _dbContext.Routines.ToListAsync(cancellationToken);

        return Result.Success(routines.AsEnumerable());
    }

    public async Task<Result<IEnumerable<RoutineExercise>>> GetAllExercisesAsync(
        CancellationToken cancellationToken = default)
    {
        var exercises = await _dbContext.RoutineExercises.ToListAsync(cancellationToken);

        return Result.Success(exercises.AsEnumerable());
    }

    public async Task<Result<IEnumerable<RoutineExercise>>> GetAllExercisesByRoutineIdAsync(
        RoutineId routineId,
        CancellationToken cancellationToken = default)
    {
        var routines = await _dbContext.RoutineExercises
            .Where(re => re.RoutineId == routineId)
            .ToListAsync(cancellationToken);

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

    public async Task<Result<RoutineExercise>> GetExerciseByIdAsync(
        RoutineExerciseId exerciseId,
        CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            await _dbContext.RoutineExercises.FirstOrDefaultAsync(r => r.Id == exerciseId, cancellationToken),
            r => r is not null,
            ApplicationErrors.Routine.NotFound)!;
    }
}
