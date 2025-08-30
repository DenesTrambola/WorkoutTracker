namespace WorkoutTracker.Application.Measurements.Queries.GetAll;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed class GetAllMeasurementsQueryHandler(
    IMeasurementRepository measurementRepository,
    IUnitOfWork unitOfWork)
    : IQueryHandler<GetAllMeasurementsQuery, IEnumerable<MeasurementResponse>>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<MeasurementResponse>>> Handle(
        GetAllMeasurementsQuery request,
        CancellationToken cancellationToken = default)
    {
        var measurementsResult = await _measurementRepository.GetAllAsync(cancellationToken);

        if (request.Name is not null)
            measurementsResult = measurementsResult.Map(m => m.Where(
                m => m.Name.Value == request.Name));

        if (request.Description is not null)
            measurementsResult = measurementsResult.Map(m => m.Where(
                m => m.Description.Text is not null && m.Description.Text == request.Description));

        if (request.Unit is not null)
            measurementsResult = measurementsResult.Map(m => m.Where(
                m => m.Unit == request.Unit));

        if (request.UserId is not null)
            measurementsResult = measurementsResult.Map(m => m.Where(
                m => m.UserId.IdValue == request.UserId));

        return measurementsResult.Map(m => m.Select(m => new MeasurementResponse
        {
            Id = m.Id.IdValue,
            Name = m.Name.Value,
            Description = m.Description.Text ?? string.Empty,
            Unit = m.Unit.ToString(),
            UserId = m.UserId.IdValue
        }));
    }
}
