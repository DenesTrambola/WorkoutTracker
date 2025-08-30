namespace WorkoutTracker.Application.Measurements.Commands.Update;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WorkoutTracker.Application.Measurements.Queries;
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
    : ICommandHandler<UpdateMeasurementCommand, MeasurementResponse>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<MeasurementResponse>> Handle(
    UpdateMeasurementCommand request,
    CancellationToken cancellationToken = default)
    {
        var measurementResult = (await TryGetMeasurementByIdAsync(request.Id, cancellationToken))
            .Map(m =>
            {
                Result<Measurement> updateResult = m;

                if (request.Name is not null)
                    updateResult = TryUpdateName(m, request.Name, cancellationToken);
                if (request.Description is not null)
                    updateResult = TryUpdateDescription(m, request.Description, cancellationToken);
                if (request.Unit is not null)
                    updateResult = TryUpdateUnit(m, request.Unit, cancellationToken);
                if (request.UserId is not null && request.UserId.HasValue)
                    updateResult = TryReassignToUser(m, (Guid)request.UserId, cancellationToken);

                return updateResult;
            });

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return measurementResult.Map(m => new MeasurementResponse
        {
            Id = m.Id.IdValue,
            Name = m.Name.Value,
            Description = m.Description.Text ?? string.Empty,
            Unit = m.Unit.ToString(),
            UserId = m.UserId.IdValue
        });
    }

    private async Task<Result<Measurement>> TryGetMeasurementByIdAsync(
        Guid measurementId,
        CancellationToken cancellationToken = default)
    {
        return await MeasurementId.FromGuid(measurementId)
            .MapAsync(id => _measurementRepository.GetByIdAsync(id, cancellationToken));
    }

    private Result<Measurement> TryUpdateName(
        Measurement measurement,
        string newName,
        CancellationToken cancellationToken = default)
    {
        return Name.Create(newName)
            .Map(measurement.UpdateName);
    }

    private Result<Measurement> TryUpdateDescription(
        Measurement measurement,
        string newDescription,
        CancellationToken cancellationToken = default)
    {
        return Description.Create(newDescription)
            .Map(measurement.UpdateDescription);
    }

    private Result<Measurement> TryUpdateUnit(
        Measurement measurement,
        string unit,
        CancellationToken cancellationToken = default)
    {
        return (unit switch
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
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return UserId.FromGuid(userId)
            .Map(measurement.ReassignToUser);
    }
}
