namespace WorkoutTracker.Infrastructure.Models;

public sealed record SmtpEmailOptions
{
    public required string Username { get; init; } = string.Empty;

    public required string Password { get; init; } = string.Empty;

    public required string Host { get; init; } = string.Empty;

    public required int Port { get; init; }

    public required bool EnableSsl { get; init; }
}
