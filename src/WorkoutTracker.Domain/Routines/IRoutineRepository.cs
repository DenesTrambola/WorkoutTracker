namespace WorkoutTracker.Domain.Routines;

using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;

public interface IRoutineRepository
    : IRepository<Routine, RoutineId>
{
}
