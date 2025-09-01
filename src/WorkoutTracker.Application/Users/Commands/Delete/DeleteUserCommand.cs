namespace WorkoutTracker.Application.Users.Commands.Delete;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record DeleteUserCommand(Guid Id)
    : ICommand;
