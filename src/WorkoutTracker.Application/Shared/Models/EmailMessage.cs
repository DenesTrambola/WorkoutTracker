namespace WorkoutTracker.Application.Shared.Models;

using WorkoutTracker.Domain.Users.ValueObjects;

public record EmailMessage(
    Email From,
    Email To,
    string? Subject = null,
    string? Body = null,
    bool IsHtml = true);
