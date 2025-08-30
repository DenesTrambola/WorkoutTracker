namespace WorkoutTracker.Persistence.Repositories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Application.Exercises.Errors;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Shared.Results;

public sealed class ExerciseRepository(
    AppDbContext dbContext)
    : IExerciseRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Result<Exercise>> AddAsync(
        Exercise entity,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Exercises.AddAsync(entity, cancellationToken);
            return Result.Success(entity);
        }
        catch (Exception)
        {
            return Result.Failure<Exercise>(ApplicationErrors.Exercise.CannotAddToDatabase);
        }
    }

    public async Task<Result> DeleteAsync(
        ExerciseId id,
        CancellationToken cancellationToken = default)
    {
        var exerciseResult = await GetByIdAsync(id, cancellationToken);

        if (exerciseResult.IsFailure)
            return Result.Failure(exerciseResult.Errors);

        try
        {
            _dbContext.Exercises.Remove(exerciseResult.ValueOrDefault()!);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Exercise.CannotDeleteFromDatabase);
        }
    }

    public async Task<Result<IEnumerable<Exercise>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var exercises = await _dbContext.Exercises.ToListAsync(cancellationToken);

        return Result.Success(exercises.AsEnumerable());
    }

    public async Task<Result<Exercise>> GetByIdAsync(
        ExerciseId id,
        CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            await _dbContext.Exercises.FirstOrDefaultAsync(m => m.Id == id, cancellationToken),
            m => m is not null,
            ApplicationErrors.Exercise.NotFound)!;
    }
}
