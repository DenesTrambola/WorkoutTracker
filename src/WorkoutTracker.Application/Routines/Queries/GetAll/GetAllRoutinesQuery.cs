namespace WorkoutTracker.Application.Routines.Queries.GetAll;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetAllRoutinesQuery
    : IQuery<IEnumerable<RoutineResponse>>
{
    public string? Name { get; init; }

    public string? Description { get; init; }

    public Guid? UserId { get; init; }
}
