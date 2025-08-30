namespace WorkoutTracker.Domain.Exercises;

using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;

public interface IExerciseRepository
    : IRepository<Exercise, ExerciseId>
{
}
