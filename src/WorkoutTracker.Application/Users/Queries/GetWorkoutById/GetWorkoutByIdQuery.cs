namespace WorkoutTracker.Application.Users.Queries.GetWorkoutById;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetWorkoutByIdQuery(Guid Id)
    : IQuery<WorkoutResponse>;
