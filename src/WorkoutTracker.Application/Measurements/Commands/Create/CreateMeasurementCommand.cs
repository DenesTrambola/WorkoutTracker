namespace WorkoutTracker.Application.Measurements.Commands.Create;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record CreateMeasurementCommand
    : ICommand
{
    public required string Name { get; init; }

    public required string Description { get; init; }

    public required string Unit { get; init; }

    public required Guid UserId { get; init; }
}
