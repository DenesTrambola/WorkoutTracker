namespace WorkoutTracker.Application.Users.Commands.AddWorkoutToUser;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record CreateWorkoutCommand
    : ICommand
{
    public required DateTime StartTime { get; init; }

    public required DateTime EndTime { get; init; }

    public required TimeSpan RestTimeBetweenExercises { get; init; }

    public required string Comment { get; init; }

    public required Guid UserId { get; init; }

    public required Guid RoutineId { get; init; }
}
