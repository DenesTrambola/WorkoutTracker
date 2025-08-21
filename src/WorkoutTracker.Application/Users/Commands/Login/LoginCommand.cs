namespace WorkoutTracker.Application.Users.Commands.Login;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record LoginCommand(string Username, string Password)
    : ICommand<LoginResponse>;
