namespace WorkoutTracker.Application.Measurements.Commands.CreateData;

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

public sealed class CreateMeasurementDataCommandHandler(
    IMeasurementRepository measurementRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateMeasurementDataCommand>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        CreateMeasurementDataCommand request,
        CancellationToken cancellationToken = default)
    {
        var measurementResult = await TryToFindMeasurementAsync(request.MeasurementId, cancellationToken);
        if (measurementResult.IsFailure)
            return measurementResult;
        var measurement = measurementResult.ValueOrDefault();

        var valueResult = MeasurementDataValue.Create(request.Value);
        var measuredOn = request.MeasuredOn;
        var commentResult = Comment.Create(request.Comment);

        var dataResult = await Result.Combine(
            valueResult, commentResult)
            .OnSuccess(() => measurement.AddData(
                valueResult.ValueOrDefault(),
                measuredOn,
                commentResult.ValueOrDefault()))
            .OnSuccessAsync(async md => await _measurementRepository.AddDataAsync(md, cancellationToken));

        if (dataResult.IsFailure)
            return dataResult;

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.MeasurementData.CannotAddToDatabase);
        }

        return dataResult;
    }

    private async Task<Result<Measurement>> TryToFindMeasurementAsync(
        Guid measurementId,
        CancellationToken cancellationToken = default)
    {
        return await MeasurementId.FromGuid(measurementId)
            .MapAsync(async mId => await _measurementRepository.GetByIdAsync(mId, cancellationToken));
    }
}
