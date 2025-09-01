namespace WorkoutTracker.Application.Exercises.Commands.Update;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record UpdateExerciseCommand
    : ICommand
{
    public required Guid Id { get; init; }

    public required string? Name {  get; init; }

    public required string? TargetMuscle { get; init; }

    public required bool? IsPublic { get; init; }

    public required Guid? UserId { get; init; }
}
