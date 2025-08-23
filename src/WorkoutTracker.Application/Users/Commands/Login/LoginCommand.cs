namespace WorkoutTracker.Application.Users.Commands.Login;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record LoginCommand
    : ICommand<LoginResponse>
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}
