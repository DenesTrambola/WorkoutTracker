namespace WorkoutTracker.Web.Presentation.Requests.Routines;

using System.ComponentModel.DataAnnotations;

public sealed record CreateRoutineDto
{
    [Required]
    public required string Name { get; init; }

    public required string Description { get; init; }

    [Required]
    public required Guid UserId { get; init; }
}
