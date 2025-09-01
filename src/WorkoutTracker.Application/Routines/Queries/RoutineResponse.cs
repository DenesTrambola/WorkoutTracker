namespace WorkoutTracker.Application.Routines.Queries;

public sealed record RoutineResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required Guid UserId { get; init; }
}
