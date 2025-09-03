namespace WorkoutTracker.Web.Presentation.Requests.Users;

public sealed record UpdateWorkoutDto
{
    public DateTime? StartTime { get; init; }

    public DateTime? EndTime { get; init; }

    public TimeSpan? RestTimeBetweenExercises { get; init; }

    public string? Comment { get; init; }

    public Guid? UserId { get; init; }

    public Guid? RoutineId { get; init; }
}
