namespace WorkoutTracker.Application.Routines.Commands.UpdateExercise;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record UpdateRoutineExerciseCommand
    : ICommand
{
    public required Guid Id { get; init; }

    public required byte? SetCount { get; init; }

    public required byte? RepCount { get; init; }

    public required TimeSpan? RestTimeBetweenSets { get; init; }

    public required string? Comment { get; init; }

    public required byte? Position { get; init; }

    public required Guid? RoutineId { get; init; }

    public required Guid? ExerciseId { get; init; }
}
