namespace WorkoutTracker.Domain.Routines;

using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Routines.ValueObjects;
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

    private RoutineExercise()
    {
        Comment = null!;
        Position = null!;
        RoutineId = null!;
        ExerciseId = null!;
    }

    private RoutineExercise(
        RoutineExerciseId id,
        byte setCount,
        byte repCount,
        TimeSpan restTimeBetweenSets,
        Comment comment,
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

    internal static Result<RoutineExercise> Create(
        byte setCount,
        byte repCount,
        TimeSpan restTimeBetweenSets,
        Comment comment,
        ExercisePosition position,
        RoutineId routineId,
        ExerciseId exerciseId)
    {
        Result<RoutineExerciseId> routineExerciseIdResult = RoutineExerciseId.New();

        return Result.Combine(
            EnsureSetCountIsValid(setCount),
            EnsureRepCountIsValid(repCount),
            EnsureRestTimeIsValid(restTimeBetweenSets),
            Comment.EnsureNotNull(comment),
            ExercisePosition.EnsureNotNull(position),
            RoutineId.EnsureNotNull(routineId),
            ExerciseId.EnsureNotNull(exerciseId),
            routineExerciseIdResult)
            .OnSuccess(() => new RoutineExercise(
                routineExerciseIdResult.ValueOrDefault(),
                setCount,
                repCount,
                restTimeBetweenSets,
                comment,
                position,
                routineId,
                exerciseId));
    }

    private static Result<byte> EnsureSetCountIsValid(byte setCount)
    {
        return Result.Ensure(
            setCount,
            sc => sc > 0,
            DomainErrors.RoutineExercise.InvalidSetCount);
    }

    private static Result<byte> EnsureRepCountIsValid(byte repCount)
    {
        return Result.Ensure(
            repCount,
            rc => rc > 0,
            DomainErrors.RoutineExercise.InvalidRepCount);
    }

    private static Result<TimeSpan> EnsureRestTimeIsValid(TimeSpan restTimeBetweenSets)
    {
        return Result.Ensure(
            restTimeBetweenSets,
            stbs => stbs > TimeSpan.Zero,
            DomainErrors.RoutineExercise.InvalidRestTimeBetweenSets);
    }

    public Result<RoutineExercise> UpdateSetCount(byte newSetCount)
    {
        return Result.Ensure(
            newSetCount,
            sc => sc > 0,
            DomainErrors.RoutineExercise.InvalidSetCount)
            .OnSuccess(sc => SetCount = sc)
            .Map(_ => this);
    }

    public Result<RoutineExercise> UpdateRepCount(byte newRepCount)
    {
        return Result.Ensure(
            newRepCount,
            rc => rc > 0,
            DomainErrors.RoutineExercise.InvalidRepCount)
            .OnSuccess(rc => RepCount = rc)
            .Map(_ => this);
    }

    public Result<RoutineExercise> UpdateRestTimeBetweenSets(TimeSpan newRestTimeBetweenSets)
    {
        return Result.Ensure(
            newRestTimeBetweenSets,
            stbs => stbs > TimeSpan.Zero,
            DomainErrors.RoutineExercise.InvalidRestTimeBetweenSets)
            .OnSuccess(stbs => RestTimeBetweenSets = stbs)
            .Map(_ => this);
    }

    public Result<RoutineExercise> UpdateComment(Comment newComment)
    {
        return Result.Ensure(
            newComment,
            c => c is not null,
            Shared.Errors.DomainErrors.Comment.Null)
            .OnSuccess(c => Comment = c)
            .Map(_ => this);
    }

    public Result<RoutineExercise> UpdatePosition(ExercisePosition newPosition)
    {
        return Result.Ensure(
            newPosition,
            p => p is not null,
            DomainErrors.ExercisePosition.Null)
            .OnSuccess(p => Position = p)
            .Map(_ => this);
    }
}
