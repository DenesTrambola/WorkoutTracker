namespace WorkoutTracker.Application.Routines.Queries.GetExerciseById;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetRoutineExerciseByIdQuery(Guid Id)
    : IQuery<RoutineExerciseResponse>;
