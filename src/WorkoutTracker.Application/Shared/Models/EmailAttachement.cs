namespace WorkoutTracker.Application.Shared.Models;

public record EmailAttachement(
    string FileName,
    byte[] Content,
    string ContentType = "application/octet-stream");
