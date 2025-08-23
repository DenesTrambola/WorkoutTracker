namespace WorkoutTracker.Infrastructure.Models;

public sealed record JwtSettings(
    string SecretKey,
    int ExpiryMinutes,
    string Issuer,
    string Audience);
