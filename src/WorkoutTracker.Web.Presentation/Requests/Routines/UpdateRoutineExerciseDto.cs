namespace WorkoutTracker.Web.Presentation.Requests.Routines;

public sealed record UpdateRoutineExerciseDto
{
    public byte? SetCount { get; init; }

    public byte? RepCount { get; init; }

    public TimeSpan? RestTimeBetweenSets { get; init; }

    public string? Comment { get; init; }

    public byte? Position { get; init; }

    public Guid? RoutineId { get; init; }

    public Guid? ExerciseId { get; init; }
}
