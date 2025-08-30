namespace WorkoutTracker.Application.Measurements.Commands.Update;

using WorkoutTracker.Application.Measurements.Queries;
using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record UpdateMeasurementCommand
    : ICommand<MeasurementResponse>
{
    public required Guid Id { get; init; }

    public string? Name { get; init; }

    public string? Description { get; init; }

    public string? Unit { get; init; }

    public Guid? UserId { get; init; }
}
