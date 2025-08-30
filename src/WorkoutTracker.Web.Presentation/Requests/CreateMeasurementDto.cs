namespace WorkoutTracker.Web.Presentation.Requests;

using System.ComponentModel.DataAnnotations;

public sealed record CreateMeasurementDto
{
    [Required]
    public required string Name { get; init; }

    public required string Description { get; init; }

    [Required]
    public required string Unit { get; init; }

    [Required]
    public required Guid UserId { get; init; }
}
