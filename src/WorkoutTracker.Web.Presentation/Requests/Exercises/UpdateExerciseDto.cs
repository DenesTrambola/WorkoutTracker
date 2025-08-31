namespace WorkoutTracker.Web.Presentation.Requests.Exercises;

public sealed record UpdateExerciseDto
{
    public string? Name { get; init; }

    public string? TargetMuscle { get; init; }

    public bool? IsPublic { get; init; }

    public Guid? UserId { get; init; }
}
