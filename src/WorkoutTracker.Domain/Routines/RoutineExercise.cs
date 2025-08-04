namespace WorkoutTracker.Domain.Routines;

using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;

public class RoutineExercise : Entity<RoutineExerciseId>
{
    public byte SetCount { get; private set; }
    public byte RepCount { get; private set; }
    public TimeSpan RestTimeBetweenSets { get; private set; }
    public Comment Comment { get; private set; }
    public ExercisePosition Position { get; private set; }
    public RoutineId RoutineId { get; private set; }
    public ExerciseId ExerciseId { get; private set; }

    public Routine Routine { get; private set; }
    public Exercise Exercise { get; private set; }

    private RoutineExercise()
    {
        Comment = null!;
        Position = null!;
        RoutineId = null!;
        ExerciseId = null!;
        Routine = null!;
        Exercise = null!;
    }

    private RoutineExercise(
        RoutineExerciseId id,
        byte setCount,
        byte repCount,
        TimeSpan restTimeBetweenSets,
        Comment comment,
        ExercisePosition position,
        Routine routine,
        Exercise exercise)
        : base(id)
    {
        SetCount = setCount;
        RepCount = repCount;
        RestTimeBetweenSets = restTimeBetweenSets;
        Comment = comment;
        Position = position;
        Routine = routine;
        Exercise = exercise;
        RoutineId = routine.Id;
        ExerciseId = exercise.Id;
    }

    internal static Result<RoutineExercise> Create(
        RoutineExerciseId id,
        byte setCount,
        byte repCount,
        TimeSpan restTimeBetweenSets,
        Comment comment,
        ExercisePosition position,
        Routine routine,
        Exercise exercise)
    {
        return new RoutineExercise(
            id,
            setCount,
            repCount,
            restTimeBetweenSets,
            comment,
            position,
            routine,
            exercise);
    }
}
