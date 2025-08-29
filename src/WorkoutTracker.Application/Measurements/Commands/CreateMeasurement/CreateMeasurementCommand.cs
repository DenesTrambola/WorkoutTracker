namespace WorkoutTracker.Application.Measurements.Commands;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record CreateMeasurementCommand
    : ICommand
{
    public required string Name { get; init; }

    public required string Description { get; init; }

    public required byte Unit { get; init; }

    public required Guid UserId { get; init; }
}
