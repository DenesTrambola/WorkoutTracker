namespace WorkoutTracker.Application.Shared.Models;

using WorkoutTracker.Domain.Users.ValueObjects;

public record EmailMessage
{
    public required Email From { get; init; }

    public required Email To { get; init; }

    public string? Subject { get; init; } = null!;

    public string? Body { get; init; } = null!;

    public bool IsHtml { get; init; } = true;
}
