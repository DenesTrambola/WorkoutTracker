namespace WorkoutTracker.Persistence.Repositories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Application.Measurements.Errors;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class MeasurementRepository(
    AppDbContext dbContext)
    : IMeasurementRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Result<Measurement>> AddAsync(
        Measurement entity,
        CancellationToken cancellationToken = default)
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

    public async Task<Result<MeasurementData>> AddDataAsync(MeasurementData data, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.MeasurementData.AddAsync(data, cancellationToken);
            return Result.Success(data);
        }
        catch (Exception)
        {
            return Result.Failure<MeasurementData>(ApplicationErrors.MeasurementData.CannotAddToDatabase);
        }
    }

    public async Task<Result> DeleteAsync(
        MeasurementId id,
        CancellationToken cancellationToken = default)
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

    public async Task<Result> DeleteDataAsync(MeasurementDataId dataId, CancellationToken cancellationToken = default)
    {
        var dataResult = await GetDataByIdAsync(dataId, cancellationToken);

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

    public async Task<Result<IEnumerable<Measurement>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var measurements = await _dbContext.Measurements.ToListAsync(cancellationToken);

        return Result.Success(measurements.AsEnumerable());
    }

    public async Task<Result<IEnumerable<Measurement>>> GetAllByUserAsync(
        UserId userId, CancellationToken
        cancellationToken = default)
    {
        var measurements = await _dbContext.Measurements
            .Where(m => m.UserId == userId)
            .ToListAsync(cancellationToken);

        return Result.Success(measurements.AsEnumerable());
    }

    public async Task<Result<IEnumerable<MeasurementData>>> GetAllDataAsync(CancellationToken cancellationToken = default)
    {
        var data = await _dbContext.MeasurementData.ToListAsync(cancellationToken);

        return Result.Success(data.AsEnumerable());
    }

    public async Task<Result<Measurement>> GetByIdAsync(
        MeasurementId id,
        CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            await _dbContext.Measurements.FirstOrDefaultAsync(m => m.Id == id, cancellationToken),
            m => m is not null,
            ApplicationErrors.Measurement.NotFound)!;
    }

    public async Task<Result<MeasurementData>> GetDataByIdAsync(MeasurementDataId dataId, CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            await _dbContext.MeasurementData.FirstOrDefaultAsync(md => md.Id == dataId, cancellationToken),
            md => md is not null,
            ApplicationErrors.MeasurementData.NotFound)!;
    }

    public async Task<Result<Name>> ValidateNameUniqueness(
        Name name,
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        return Result.Ensure(
            !(await _dbContext.Measurements.AnyAsync(
                m => m.Name == name && m.UserId == userId, cancellationToken)),
            Application.Shared.Errors.ApplicationErrors.Name.Taken)
            .OnSuccess(() => name);
    }
}
