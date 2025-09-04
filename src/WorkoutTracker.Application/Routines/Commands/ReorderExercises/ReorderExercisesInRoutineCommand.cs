namespace WorkoutTracker.Application.Routines.Commands.ReorderExercises;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record ReorderExercisesInRoutineCommand
    : ICommand
{
    public required Guid RoutineId { get; init; }

    public required IReadOnlyList<Guid> ExerciseIdsInOrder { get; init; }
}
