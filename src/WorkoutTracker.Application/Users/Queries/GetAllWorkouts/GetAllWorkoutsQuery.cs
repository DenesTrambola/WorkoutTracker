namespace WorkoutTracker.Application.Users.Queries.GetAllWorkouts;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetAllWorkoutsQuery
    : IQuery<IEnumerable<WorkoutResponse>>
{
    public DateTime? StartTime { get; init; }

    public DateTime? EndTime { get; init; }

    public TimeSpan? Duration { get; init; }

    public TimeSpan? RestTimeBetweenExercises { get; init; }

    public string? Comment { get; init; }

    public Guid? UserId { get; init; }

    public Guid? RoutineId { get; init; }
}
