namespace WorkoutTracker.Application.Users.Models;

public record AccessToken
{
    public required string Token { get; init; } = null!;

    public required DateTime ExpiresAt { get; init; }
}
