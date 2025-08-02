namespace WorkoutTracker.Domain.Routines;

using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.ValueObjects;

public class RoutineExercise : Entity<RoutineExerciseId>
{
    public byte SetCount { get; private set; }
    public byte RepCount { get; private set; }
    public TimeSpan RestTimeBetweenSets { get; private set; }
    public Comment? Comment { get; private set; }
    public ExercisePosition Position { get; private set; }
    public RoutineId RoutineId { get; private set; }
    public ExerciseId ExerciseId { get; private set; }

    private RoutineExercise(
        RoutineExerciseId id,
        byte setCount,
        byte repCount,
        TimeSpan restTimeBetweenSets,
        Comment? comment,
        ExercisePosition position,
        RoutineId routineId,
        ExerciseId exerciseId)
        : base(id)
    {
        SetCount = setCount;
        RepCount = repCount;
        RestTimeBetweenSets = restTimeBetweenSets;
        Comment = comment;
        Position = position;
        RoutineId = routineId;
        ExerciseId = exerciseId;
    }

    internal static RoutineExercise Create(
        RoutineExerciseId id,
        byte setCount,
        byte repCount,
        TimeSpan restTimeBetweenSets,
        Comment? comment,
        ExercisePosition position,
        RoutineId routineId,
        ExerciseId exerciseId)
        => new RoutineExercise(
            id,
            setCount,
            repCount,
            restTimeBetweenSets,
            comment,
            position,
            routineId,
            exerciseId);
}
