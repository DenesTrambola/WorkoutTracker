namespace WorkoutTracker.Infrastructure.Models;

public sealed record SmtpEmailSettings(
    string Username,
    string Password,
    string Host,
    int Port = 587,
    bool EnableSsl = true);
