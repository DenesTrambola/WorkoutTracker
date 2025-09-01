namespace WorkoutTracker.Application.Measurements.Commands.CreateData;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record CreateMeasurementDataCommand
    : ICommand
{
    public required float Value { get; init; }

    public required DateTime MeasuredOn { get; init; }

    public required string Comment { get; init; }

    public required Guid MeasurementId { get; init; }
}
