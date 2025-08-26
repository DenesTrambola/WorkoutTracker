namespace WorkoutTracker.Infrastructure.Models;

public sealed record JwtOptions
{
    public required string SecretKey { get; init; } = string.Empty;

    public required int ExpiryMinutes { get; init; }

    public required string Issuer { get; init; } = string.Empty;

    public required string Audience { get; init; } = string.Empty;
}
