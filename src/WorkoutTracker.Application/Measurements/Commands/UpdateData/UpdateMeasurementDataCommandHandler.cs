namespace WorkoutTracker.Application.Measurements.Commands.UpdateData;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Measurements.Errors;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Measurements.ValueObjects;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;

public sealed class UpdateMeasurementDataCommandHandler(
    IMeasurementRepository measurementRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateMeasurementDataCommand>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateMeasurementDataCommand request, CancellationToken cancellationToken)
    {
        var dataResult = (await TryGetMeasurementDataByIdAsync(request.Id, cancellationToken))
            .Map(md =>
            {
                return Result.Combine(
                    TryUpdateValue(md, request.Value, cancellationToken),
                    TryUpdateMeasuredOn(md, request.MeasuredOn, cancellationToken),
                    TryUpdateComment(md, request.Comment, cancellationToken));
            });

        if (dataResult.IsFailure)
            return Result.Failure(dataResult.Errors);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.MeasurementData.CannotUpdateInDatabase);
        }

        return dataResult;
    }

    private async Task<Result<MeasurementData>> TryGetMeasurementDataByIdAsync(
        Guid dataId,
        CancellationToken cancellationToken = default)
    {
        return await MeasurementDataId.FromGuid(dataId)
            .MapAsync(async id => await _measurementRepository.GetDataByIdAsync(id, cancellationToken));
    }

    private Result<MeasurementData> TryUpdateValue(
        MeasurementData data,
        float? newValue,
        CancellationToken cancellationToken = default)
    {
        return newValue is null
            ? data
            : MeasurementDataValue.Create(newValue.Value)
            .Map(data.UpdateValue);
    }

    private Result<MeasurementData> TryUpdateMeasuredOn(
        MeasurementData data,
        DateTime? newMeasuredOn,
        CancellationToken cancellationToken = default)
    {
        return newMeasuredOn is null
            ? data
            : data.UpdateMeasuredOn(newMeasuredOn.Value);
    }

    private Result<MeasurementData> TryUpdateComment(
        MeasurementData data,
        string? newComment,
        CancellationToken cancellationToken = default)
    {
        return newComment is null
            ? data
            : Comment.Create(newComment)
            .Map(data.UpdateComment);
    }

    private Result<MeasurementData> TryReassignToMeasurement(
        MeasurementData data,
        Guid? newMeasurementId,
        CancellationToken cancellationToken = default)
    {
        return newMeasurementId is null
            ? data
            : MeasurementId.FromGuid(newMeasurementId.Value)
            .Map(data.ReassignToMeasurement);
    }
}
