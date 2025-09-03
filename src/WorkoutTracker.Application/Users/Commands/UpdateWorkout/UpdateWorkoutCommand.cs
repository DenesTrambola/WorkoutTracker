namespace WorkoutTracker.Application.Users.Commands.UpdateWorkout;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record UpdateWorkoutCommand
    : ICommand
{
    public required Guid Id { get; init; }

    public required DateTime? StartTime { get; init; }

    public required DateTime? EndTime { get; init; }

    public required TimeSpan? RestTimeBetweenExercises { get; init; }

    public required string? Comment { get; init; }

    public required Guid? UserId { get; init; }

    public required Guid? RoutineId { get; init; }
}
