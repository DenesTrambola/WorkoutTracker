namespace WorkoutTracker.Persistence.Repositories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Application.Measurements.Errors;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class MeasurementRepository : IMeasurementRepository
{
    private readonly AppDbContext _dbContext;

    public MeasurementRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Measurement>> AddAsync(Measurement entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Measurements.AddAsync(entity, cancellationToken);
            return Result.Success(entity);
        }
        catch (Exception)
        {
            return Result.Failure<Measurement>(ApplicationErrors.Measurement.CannotAddToDatabase);
        }
    }

    public async Task<Result<IEnumerable<Measurement>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var measurements = await _dbContext.Measurements.ToListAsync(cancellationToken);
        return Result.Success(measurements.AsEnumerable());
    }

    public async Task<Result<Measurement>> GetByIdAsync(MeasurementId id, CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            await _dbContext.Measurements.FirstOrDefaultAsync(m => m.Id == id, cancellationToken),
            m => m is not null,
            ApplicationErrors.Measurement.NotFound)!;
    }

    public async Task<Result> RemoveAsync(MeasurementId id, CancellationToken cancellationToken = default)
    {
        var measurementResult = await GetByIdAsync(id, cancellationToken);

        if (measurementResult.IsFailure)
            return Result.Failure(measurementResult.Errors);

        try
        {
            _dbContext.Measurements.Remove(measurementResult.ValueOrDefault()!);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Measurement.CannotDeleteFromDatabase);
        }
    }

    public async Task<Result<Name>> ValidateNameUniqueness(Name name, UserId userId, CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            !(await _dbContext.Measurements.AnyAsync(
                m => m.Name == name && m.UserId == userId, cancellationToken)),
            Application.Shared.Errors.ApplicationErrors.Name.Taken)
            .OnSuccess(() => name);
    }
}

