namespace WorkoutTracker.Web.Presentation.Requests.Measurements;

using System.ComponentModel.DataAnnotations;

public sealed record CreateMeasurementDataDto
{
    [Required]
    public required float Value { get; init; }

    [Required]
    public required DateTime MeasuredOn { get; init; }

    public required string Comment { get; init; }
}
