namespace WorkoutTracker.Application.Measurements.Commands.Update;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record UpdateMeasurementCommand
    : ICommand
{
    public required Guid Id { get; init; }

    public string? Name { get; init; }

    public string? Description { get; init; }

    public string? Unit { get; init; }

    public Guid? UserId { get; init; }
}
