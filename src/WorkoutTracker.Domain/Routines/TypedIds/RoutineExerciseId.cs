namespace WorkoutTracker.Domain.Routines.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;

public record RoutineExerciseId : StronglyTypedId<Guid>
{
    protected RoutineExerciseId(Guid id)
        : base(id)
    {
    }

    public static RoutineExerciseId New()
    {
        return new RoutineExerciseId(Guid.NewGuid());
    }
}
