namespace WorkoutTracker.Web.Presentation.Requests.Exercises;

using System.ComponentModel.DataAnnotations;

public sealed record CreateExerciseDto
{
    [Required]
    public required string Name { get; init; }

    [Required]
    public required string TargetMuscle { get; init; }

    [Required]
    public required bool IsPublic { get; init; }

    [Required]
    public required Guid UserId { get; init; }
}
