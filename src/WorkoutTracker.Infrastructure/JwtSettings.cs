namespace WorkoutTracker.Infrastructure;

public sealed record JwtSettings(
    string SecretKey,
    int ExpiryMinutes,
    string Issuer,
    string Audience);
