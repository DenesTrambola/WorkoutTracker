namespace WorkoutTracker.Application.Measurements.Queries.GetAllData;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetAllMeasurementDataQuery
    : IQuery<IEnumerable<MeasurementDataResponse>>
{
    public required float? Value { get; init; }

    public required DateTime? MeasuredOn { get; init; }

    public required string? Comment { get; init; }

    public required Guid? MeasurementId { get; init; }
}
