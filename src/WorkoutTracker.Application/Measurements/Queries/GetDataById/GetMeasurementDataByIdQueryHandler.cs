namespace WorkoutTracker.Application.Measurements.Queries.GetDataById;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Results;

public sealed class GetMeasurementDataByIdQueryHandler(
    IMeasurementRepository measurementRepository)
    : IQueryHandler<GetMeasurementDataByIdQuery, MeasurementDataResponse>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;

    public async Task<Result<MeasurementDataResponse>> Handle(GetMeasurementDataByIdQuery request, CancellationToken cancellationToken)
    {
        var dataResult = await MeasurementDataId.FromGuid(request.Id)
             .MapAsync(async id => await _measurementRepository.GetDataByIdAsync(id, cancellationToken));

        return dataResult.Map(md => new MeasurementDataResponse
        {
            Id = md.Id.IdValue,
            Value = md.Value.Value,
            MeasuredOn = md.MeasuredOn,
            Comment = md.Comment.Text ?? string.Empty,
            MeasurementId = md.MeasurementId.IdValue,
        });
    }
}
