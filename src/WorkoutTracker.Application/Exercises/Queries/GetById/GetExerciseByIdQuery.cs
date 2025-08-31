namespace WorkoutTracker.Application.Exercises.Queries.GetById;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetExerciseByIdQuery(Guid Id)
    : IQuery<ExerciseResponse>;
