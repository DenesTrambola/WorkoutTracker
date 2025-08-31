namespace WorkoutTracker.Application.Exercises.Queries;

public sealed record ExerciseResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string TargetMuscle { get; init; }

    public required bool IsPublic { get; init; }

    public required Guid UserId { get; init; }
}
