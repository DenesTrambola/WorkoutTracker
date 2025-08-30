namespace WorkoutTracker.Web.Presentation.Requests.Measurements;

using System.ComponentModel.DataAnnotations;

public sealed record UpdateMeasurementDto
{
    public string? Name { get; init; }

    public string? Description { get; init; }

    public string? Unit { get; init; }

    public Guid? UserId { get; init; }
}
