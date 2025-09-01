namespace WorkoutTracker.Application.Measurements.Commands.DeleteData;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Measurements.Errors;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed class DeleteMeasurementDataCommandHandler(
    IMeasurementRepository measurementRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteMeasurementDataCommand>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeleteMeasurementDataCommand request,
        CancellationToken cancellationToken = default)
    {
        var dataIdResult = MeasurementDataId.FromGuid(request.Id);

        var measurementResult = await dataIdResult.MapAsync(
            async id => await _measurementRepository.GetDataByIdAsync(id, cancellationToken));

        var deleteResult = await measurementResult.OnSuccessAsync(
            async md => await _measurementRepository.DeleteDataAsync(dataIdResult.ValueOrDefault()));

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.MeasurementData.CannotDeleteFromDatabase);
        }

        return deleteResult;
    }
}
