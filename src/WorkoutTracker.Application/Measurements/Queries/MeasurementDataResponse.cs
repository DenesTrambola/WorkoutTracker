namespace WorkoutTracker.Application.Measurements.Queries;

public sealed record MeasurementDataResponse
{
    public required Guid Id { get; init; }

    public required float Value { get; init; }

    public required DateTime MeasuredOn { get; init; }

    public required string Comment { get; init; }

    public required Guid MeasurementId { get; init; }
}
