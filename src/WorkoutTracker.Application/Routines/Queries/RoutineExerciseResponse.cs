namespace WorkoutTracker.Application.Routines.Queries;

public sealed record RoutineExerciseResponse
{
    public required Guid Id { get; init; }

    public required byte SetCount { get; init; }

    public required byte RepCount { get; init; }

    public required TimeSpan RestTimeBetweenSets { get; init; }

    public required string Comment { get; init; }

    public required byte Position { get; init; }

    public required Guid RoutineId { get; init; }

    public required Guid ExerciseId { get; init; }
}
