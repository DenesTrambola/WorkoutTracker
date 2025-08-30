namespace WorkoutTracker.Application.Measurements.Commands.Delete;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record DeleteMeasurementCommand(Guid Id)
    : ICommand;
