namespace WorkoutTracker.Application.Measurements.Commands.Delete;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Measurements.Errors;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed class DeleteMeasurementCommandHandler(
    IMeasurementRepository measurementRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteMeasurementCommand>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeleteMeasurementCommand request,
        CancellationToken cancellationToken = default)
    {
        var measurementIdResult = MeasurementId.FromGuid(request.Id);

        var measurementResult = await measurementIdResult.MapAsync(
            async id => await _measurementRepository.GetByIdAsync(id, cancellationToken));

        var deleteResult = await measurementResult.OnSuccessAsync(
            async m => await _measurementRepository.DeleteAsync(measurementIdResult.ValueOrDefault()));

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Measurement.CannotDeleteFromDatabase);
        }

        return deleteResult;
    }
}
