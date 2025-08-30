namespace WorkoutTracker.Application.Measurements.Queries.GetById;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Results;

public sealed class GetMeasurementByIdQueryHandler(
    IMeasurementRepository measurementRepository)
    : IQueryHandler<GetMeasurementByIdQuery, MeasurementResponse>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;

    public async Task<Result<MeasurementResponse>> Handle(
        GetMeasurementByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var measurementResult = await MeasurementId.FromGuid(request.Id)
             .MapAsync(async id => await _measurementRepository.GetByIdAsync(id, cancellationToken));

        return measurementResult.Map(m => new MeasurementResponse
        {
            Id = m.Id.IdValue,
            Name = m.Name.Value,
            Description = m.Description.Text ?? string.Empty,
            Unit = m.Unit.ToString(),
            UserId = m.UserId.IdValue
        });
    }
}
