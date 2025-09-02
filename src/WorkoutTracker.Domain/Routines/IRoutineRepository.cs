namespace WorkoutTracker.Domain.Routines;

using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public interface IRoutineRepository
    : IRepository<Routine, RoutineId>
{
    Task<Result<RoutineExercise>> AddExerciseAsync(
        RoutineExercise exercise,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteExerciseAsync(
        RoutineExerciseId exerciseId,
        CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<RoutineExercise>>> GetAllExercisesAsync(
        CancellationToken cancellationToken = default);

    Task<Result<RoutineExercise>> GetExerciseByIdAsync(
        RoutineExerciseId exerciseId,
        CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<RoutineExercise>>> GetAllExercisesByRoutineIdAsync(
        RoutineId routineId,
        CancellationToken cancellationToken = default);
}
