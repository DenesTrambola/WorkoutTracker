namespace WorkoutTracker.Application.Measurements.Commands.UpdateData;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record UpdateMeasurementDataCommand
    : ICommand
{
    public required Guid Id { get; init; }

    public required float? Value { get; init; }

    public required DateTime? MeasuredOn { get; init; }

    public required string? Comment { get; init; }

    public required Guid? MeasurementId { get; init; }
}
