namespace WorkoutTracker.Application.Measurements.Queries;

public sealed record MeasurementListResponse
{
    public required IEnumerable<MeasurementResponse> Measurements { get; init; }
}
