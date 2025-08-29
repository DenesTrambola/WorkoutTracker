namespace WorkoutTracker.Application.Users.Commands.Login;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record LoginCommand
    : ICommand<LoginResponse>
{
    public required string Username { get; init; }

    public required string Password { get; init; }
}
