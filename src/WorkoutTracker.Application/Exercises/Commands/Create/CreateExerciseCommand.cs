namespace WorkoutTracker.Application.Exercises.Commands.Create;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record CreateExerciseCommand
    : ICommand
{
    public required string Name { get; init; }

    public required string TargetMuscle { get; init; }

    public required bool IsPublic { get; init; }

    public required Guid UserId { get; init; }
}
