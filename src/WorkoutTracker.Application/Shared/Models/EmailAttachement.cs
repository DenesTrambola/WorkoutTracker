namespace WorkoutTracker.Application.Shared.Models;

public record EmailAttachement
{
    public string FileName { get; init; } = default!;
    public byte[] Content { get; init; } = default!;
    public string ContentType { get; init; } = "application/octet-stream";
}
