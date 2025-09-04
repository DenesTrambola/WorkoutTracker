namespace WorkoutTracker.Web.Presentation.Requests.Routines;

using System.ComponentModel.DataAnnotations;

public sealed record ReorderExerciseInRoutineDto
{
    [Required]
    public required IReadOnlyList<Guid> ExerciseIds { get; init; }
}
