namespace WorkoutTracker.Application.Shared.Models;

public record EmailMessage
{
    public string From { get; init; } = default!;
    public string To { get; init; } = default!;
    public string? Subject { get; init; }
    public string? Body { get; init; }
    public bool IsHtml { get; init; } = true;
    public IReadOnlyCollection<EmailAttachement>? Attachements { get; init; }
}
