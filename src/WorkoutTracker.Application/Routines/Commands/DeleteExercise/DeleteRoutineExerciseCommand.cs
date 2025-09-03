namespace WorkoutTracker.Application.Routines.Commands.DeleteExercise;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record DeleteRoutineExerciseCommand(Guid Id)
    : ICommand;
