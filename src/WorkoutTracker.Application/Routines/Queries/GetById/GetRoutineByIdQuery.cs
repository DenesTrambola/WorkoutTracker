namespace WorkoutTracker.Application.Routines.Queries.GetById;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetRoutineByIdQuery(Guid Id)
    : IQuery<RoutineResponse>;
