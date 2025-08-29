namespace WorkoutTracker.Application.Measurements.Queries.GetAllMeasurements;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed class GetAllMeasurementsQueryHandler(
    IMeasurementRepository measurementRepository,
    IUnitOfWork unitOfWork)
    : IQueryHandler<GetAllMeasurementsQuery, MeasurementListResponse>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<MeasurementListResponse>> Handle(
        GetAllMeasurementsQuery request,
        CancellationToken cancellationToken = default)
    {
        var measurements = await _measurementRepository.GetAllAsync(cancellationToken);

        return measurements.Map(measurements => new MeasurementListResponse
        {
            Measurements = measurements.Select(m => new MeasurementResponse
            {
                Id = m.Id.IdValue,
                Name = m.Name.Value,
                Description = m.Description.Text ?? string.Empty,
                Unit = (byte)m.Unit,
                UserId = m.UserId.IdValue
            })
        });
    }
}
