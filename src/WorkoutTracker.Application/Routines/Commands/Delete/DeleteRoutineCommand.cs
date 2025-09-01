namespace WorkoutTracker.Application.Routines.Commands.Delete;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record DeleteRoutineCommand(Guid Id)
    : ICommand;
