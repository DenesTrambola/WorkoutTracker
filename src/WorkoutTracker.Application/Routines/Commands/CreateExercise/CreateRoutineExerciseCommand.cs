namespace WorkoutTracker.Application.Routines.Commands.CreateExercise;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record CreateRoutineExerciseCommand
    : ICommand
{
    public required byte SetCount { get; init; }

    public required byte RepCount { get; init; }

    public required TimeSpan RestTimeBetweenSets { get; init; }

    public required string Comment { get; init; }

    public required Guid RoutineId { get; init; }

    public required Guid ExerciseId { get; init; }
}
