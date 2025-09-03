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
            stbs => stbs >= TimeSpan.Zero,
            DomainErrors.RoutineExercise.InvalidRestTimeBetweenSets);
    }

    public Result<RoutineExercise> UpdateSetCount(byte newSetCount)
    {
        return EnsureSetCountIsValid(newSetCount)
            .OnSuccess(sc =>
            {
                if (SetCount != sc)
                    SetCount = sc;
            })
            .Map(_ => this);
    }

    public Result<RoutineExercise> UpdateRepCount(byte newRepCount)
    {
        return EnsureRepCountIsValid(newRepCount)
            .OnSuccess(rc =>
            {
                if (RepCount != rc)
                    RepCount = rc;
            })
            .Map(_ => this);
    }

    public Result<RoutineExercise> UpdateRestTimeBetweenSets(TimeSpan newRestTimeBetweenSets)
    {
        return EnsureRestTimeIsValid(newRestTimeBetweenSets)
            .OnSuccess(rtbs =>
            {
                if (RestTimeBetweenSets != rtbs)
                    RestTimeBetweenSets = rtbs;
            })
            .Map(_ => this);
    }

    public Result<RoutineExercise> UpdateComment(Comment newComment)
    {
        return Comment.EnsureNotNull(newComment)
            .OnSuccess(c =>
            {
                if (Comment != c)
                    Comment = c;
            })
            .Map(_ => this);
    }

    public Result<RoutineExercise> UpdatePosition(ExercisePosition newPosition)
    {
        return ExercisePosition.EnsureNotNull(newPosition)
            .OnSuccess(p =>
            {
                if (Position != p)
                    Position = p;
            })
            .Map(_ => this);
    }

    public Result<RoutineExercise> ReassignToRoutine(RoutineId newRoutineId)
    {
        return RoutineId.EnsureNotNull(newRoutineId)
            .OnSuccess(r =>
            {
                if (RoutineId != r)
                    RoutineId = r;
            })
            .Map(_ => this);
    }

    public Result<RoutineExercise> ReassignToExercise(ExerciseId newExerciseId)
    {
        return ExerciseId.EnsureNotNull(newExerciseId)
            .OnSuccess(r =>
            {
                if (ExerciseId != r)
                    ExerciseId = r;
            })
            .Map(_ => this);
    }
}
