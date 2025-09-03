namespace WorkoutTracker.Web.Presentation.Requests.Users;

using System.ComponentModel.DataAnnotations;

public sealed record CreateWorkoutDto
{
    [Required]
    public required DateTime StartTime { get; init; }

    [Required]
    public required DateTime EndTime { get; init; }

    [Required]
    public required TimeSpan RestTimeBetweenExercises { get; init; }

    public required string Comment { get; init; }

    [Required]
    public required Guid UserId { get; init; }

    [Required]
    public required Guid RoutineId { get; init; }
}
