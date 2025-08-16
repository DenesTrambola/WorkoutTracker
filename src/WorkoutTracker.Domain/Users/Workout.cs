namespace WorkoutTracker.Domain.Users;

using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.TypedIds;

public class Workout : Entity<WorkoutId>
{
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public TimeSpan Duration => EndTime - StartTime;
    public TimeSpan RestTimeBetweenExercises { get; private set; }
    public Comment Comment { get; private set; }
    public UserId UserId { get; private set; }
    public RoutineId RoutineId { get; private set; }

    private Workout()
    {
        Comment = null!;
        UserId = null!;
        RoutineId = null!;
    }

    private Workout(
        WorkoutId id,
        DateTime startTime,
        DateTime endTime,
        TimeSpan restTimeBetweenExercises,
        Comment comment,
        UserId userId,
        RoutineId routineId)
        : base(id)
    {
        StartTime = startTime;
        EndTime = endTime;
        RestTimeBetweenExercises = restTimeBetweenExercises;
        UserId = userId;
        RoutineId = routineId;
        Comment = comment;
    }

    internal static Result<Workout> Create(
        DateTime startTime,
        DateTime endTime,
        TimeSpan restTimeBetweenExercises,
        Comment comment,
        UserId userId,
        RoutineId routineId)
    {
        Result<WorkoutId> workoutIdResult = WorkoutId.New();

        return Result.Combine(
            EnsureTimeIsValid(startTime, endTime),
            EnsureRestTimeIsValid(restTimeBetweenExercises),
            Comment.EnsureNotNull(comment),
            UserId.EnsureNotNull(userId),
            RoutineId.EnsureNotNull(routineId),
            workoutIdResult)
            .OnSuccess(() => new Workout(
                workoutIdResult.ValueOrDefault(),
                startTime,
                endTime,
                restTimeBetweenExercises,
                comment,
                userId,
                routineId));
    }

    private static Result<(DateTime, DateTime)> EnsureTimeIsValid(DateTime startTime, DateTime endTime)
    {
        return Result.Ensure(
            (startTime, endTime),
            t => t.startTime < t.endTime,
            DomainErrors.Workout.InvalidStartEndTime);
    }

    private static Result<TimeSpan> EnsureRestTimeIsValid(TimeSpan restTimeBetweenExercises)
    {
        return Result.Ensure(
            restTimeBetweenExercises,
            r => r >= TimeSpan.Zero,
            DomainErrors.Workout.InvalidRestTime);
    }

    public Result<Workout> UpdateStartTime(DateTime newStartTime)
    {
        return Result.Ensure(
            newStartTime,
            st => st < EndTime,
            DomainErrors.Workout.InvalidStartEndTime)
            .OnSuccess(st => StartTime = st)
            .Map(_ => this);
    }

    public Result<Workout> UpdateEndTime(DateTime newEndTime)
    {
        return Result.Ensure(
            newEndTime,
            et => et > StartTime,
            DomainErrors.Workout.InvalidStartEndTime)
            .OnSuccess(et => EndTime = et)
            .Map(_ => this);
    }

    public Result<Workout> UpdateComment(Comment comment)
    {
        return Comment.EnsureNotNull(comment)
            .OnSuccess(c => Comment = c)
            .Map(_ => this);
    }
}
