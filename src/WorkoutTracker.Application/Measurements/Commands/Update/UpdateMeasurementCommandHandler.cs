namespace WorkoutTracker.Application.Measurements.Commands.Update;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WorkoutTracker.Application.Measurements.Errors;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class UpdateMeasurementCommandHandler(
    IMeasurementRepository measurementRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateMeasurementCommand>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
    UpdateMeasurementCommand request,
    CancellationToken cancellationToken = default)
    {
        var measurementResult = (await TryGetMeasurementByIdAsync(request.Id, cancellationToken))
            .Map(m =>
            {
                return Result.Combine(
                    TryUpdateName(m, request.Name, cancellationToken),
                    TryUpdateDescription(m, request.Description, cancellationToken),
                    TryUpdateUnit(m, request.Unit, cancellationToken),
                    TryReassignToUser(m, request.UserId, cancellationToken));
            });

        if (measurementResult.IsFailure)
            return Result.Failure(measurementResult.Errors);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Measurement.CannotUpdateInDatabase);
        }

        return measurementResult;
    }

    private async Task<Result<Measurement>> TryGetMeasurementByIdAsync(
        Guid measurementId,
        CancellationToken cancellationToken = default)
    {
        return await MeasurementId.FromGuid(measurementId)
            .MapAsync(async id => await _measurementRepository.GetByIdAsync(id, cancellationToken));
    }

    private Result<Measurement> TryUpdateName(
        Measurement measurement,
        string? newName,
        CancellationToken cancellationToken = default)
    {
        return newName is null
            ? measurement
            : Name.Create(newName)
            .Map(measurement.UpdateName);
    }

    private Result<Measurement> TryUpdateDescription(
        Measurement measurement,
        string? newDescription,
        CancellationToken cancellationToken = default)
    {
        return newDescription is null
            ? measurement
            : Description.Create(newDescription)
            .Map(measurement.UpdateDescription);
    }

    private Result<Measurement> TryUpdateUnit(
        Measurement measurement,
        string? unit,
        CancellationToken cancellationToken = default)
    {
        return unit is null
            ? measurement
            : (unit switch
            {
                "Kilogram" => Result.Success(MeasurementUnit.Kilogram),
                "Centimeter" => Result.Success(MeasurementUnit.Centimeter),
                "Percentage" => Result.Success(MeasurementUnit.Percentage),
                _ => Result.Failure<MeasurementUnit>(DomainErrors.MeasurementUnit.Invalid)
            })
            .Map(measurement.UpdateUnit);
    }

    private Result<Measurement> TryReassignToUser(
        Measurement measurement,
        Guid? newUserId,
        CancellationToken cancellationToken = default)
    {
        return newUserId is null || !newUserId.HasValue
            ? measurement
            : UserId.FromGuid(newUserId.Value)
            .Map(measurement.ReassignToUser);
    }
}
