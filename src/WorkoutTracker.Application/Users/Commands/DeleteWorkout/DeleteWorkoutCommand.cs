namespace WorkoutTracker.Application.Users.Commands.DeleteWorkout;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record DeleteWorkoutCommand(Guid Id)
    : ICommand;
