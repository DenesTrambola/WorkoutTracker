namespace WorkoutTracker.Web.Presentation.Requests.Routines;

using System.ComponentModel.DataAnnotations;

public sealed record CreateRoutineExerciseDto
{
    [Required]
    public required byte SetCount { get; init; }

    [Required]
    public required byte RepCount { get; init; }

    [Required]
    public required TimeSpan RestTimeBetweenSets { get; init; }

    public required string Comment { get; init; }

    [Required]
    public required Guid RoutineId { get; init; }

    [Required]
    public required Guid ExerciseId { get; init; }
}
