namespace WorkoutTracker.Application.Measurements.Commands.DeleteData;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record DeleteMeasurementDataCommand(Guid Id)
    : ICommand;
