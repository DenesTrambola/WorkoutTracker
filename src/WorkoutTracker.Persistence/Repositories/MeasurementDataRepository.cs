namespace WorkoutTracker.Persistence.Repositories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Application.Measurements.Errors;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Results;

public sealed class MeasurementDataRepository(
    AppDbContext dbContext)
    : IMeasurementDataRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Result<MeasurementData>> AddAsync(
        MeasurementData entity,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.MeasurementData.AddAsync(entity, cancellationToken);
            return Result.Success(entity);
        }
        catch (Exception)
        {
            return Result.Failure<MeasurementData>(ApplicationErrors.MeasurementData.CannotAddToDatabase);
        }
    }

    public async Task<Result> DeleteAsync(
        MeasurementDataId id,
        CancellationToken cancellationToken = default)
    {
        var dataResult = await GetByIdAsync(id, cancellationToken);

        if (dataResult.IsFailure)
            return Result.Failure(dataResult.Errors);

        try
        {
            _dbContext.MeasurementData.Remove(dataResult.ValueOrDefault()!);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.MeasurementData.CannotDeleteFromDatabase);
        }
    }

    public async Task<Result<IEnumerable<MeasurementData>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var data = await _dbContext.MeasurementData.ToListAsync(cancellationToken);

        return Result.Success(data.AsEnumerable());
    }

    public async Task<Result<MeasurementData>> GetByIdAsync(
        MeasurementDataId id,
        CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            await _dbContext.MeasurementData.FirstOrDefaultAsync(md => md.Id == id, cancellationToken),
            md => md is not null,
            ApplicationErrors.Measurement.NotFound)!;
    }
}
