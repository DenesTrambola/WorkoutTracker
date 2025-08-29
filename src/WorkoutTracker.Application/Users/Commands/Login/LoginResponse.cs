namespace WorkoutTracker.Application.Users.Commands.Login;

public sealed record LoginResponse
{
    public required Guid UserId { get; init; }

    public required string Username { get; init; }

    public required string Email { get; init; }

    public required string Token { get; init; }

    public required DateTime ExpiresAt { get; init; }
}
