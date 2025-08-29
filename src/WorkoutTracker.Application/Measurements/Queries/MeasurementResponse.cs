namespace WorkoutTracker.Application.Measurements.Queries;

public sealed record MeasurementResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required byte Unit { get; init; }

    public required Guid UserId { get; init; }
}
