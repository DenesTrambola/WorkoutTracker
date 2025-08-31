namespace WorkoutTracker.Application.Exercises.Commands.Delete;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record DeleteExerciseCommand(Guid Id)
    : ICommand;
