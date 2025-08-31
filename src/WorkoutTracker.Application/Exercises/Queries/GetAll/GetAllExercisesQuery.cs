namespace WorkoutTracker.Application.Exercises.Queries.GetAll;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetAllExercisesQuery
    : IQuery<IEnumerable<ExerciseResponse>>
{
    public string? Name { get; init; }

    public string? TargetMuscle { get; init; }

    public bool? IsPublic { get; init; }

    public Guid? UserId { get; init; }
}
