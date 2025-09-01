namespace WorkoutTracker.Application.Measurements.Queries.GetAllData;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Shared.Results;

public sealed class GetAllMeasurementDataQueryHandler(
    IMeasurementRepository measurementRepository)
    : IQueryHandler<GetAllMeasurementDataQuery, IEnumerable<MeasurementDataResponse>>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;

    public async Task<Result<IEnumerable<MeasurementDataResponse>>> Handle(
        GetAllMeasurementDataQuery request,
        CancellationToken cancellationToken = default)
    {
        var dataResult = await _measurementRepository.GetAllDataAsync(cancellationToken);

        if (request.Value is not null)
            dataResult = dataResult.Map(md => md.Where(
                md => md.Value.Value == request.Value));

        if (request.MeasuredOn is not null)
            dataResult = dataResult.Map(md => md.Where(
                md => md.MeasuredOn == request.MeasuredOn));

        if (request.Comment is not null)
            dataResult = dataResult.Map(md => md.Where(
                md => md.Comment.Text == request.Comment));

        if (request.MeasurementId is not null)
            dataResult = dataResult.Map(md => md.Where(
                md => md.MeasurementId.IdValue == request.MeasurementId));

        return dataResult.Map(d => d.Select(md => new MeasurementDataResponse
        {
            Id = md.Id.IdValue,
            Value = md.Value.Value,
            MeasuredOn = md.MeasuredOn,
            Comment = md.Comment.Text ?? string.Empty,
            MeasurementId = md.MeasurementId.IdValue
        }));
    }
}
