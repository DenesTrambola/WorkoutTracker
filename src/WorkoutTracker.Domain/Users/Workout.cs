namespace WorkoutTracker.Domain.Users;

using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public class Workout : Entity<WorkoutId>
{
    public TimeSpan Duration { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public TimeSpan RestTimeBetweenExercises { get; private set; }
    public Comment? Comment { get; private set; }
    public UserId UserId { get; private set; }
    public RoutineId RoutineId { get; private set; }

    private Workout(
        WorkoutId id,
        TimeSpan duration,
        DateTime endDateTime,
        TimeSpan restTimeBetweenExercises,
        UserId userId,
        RoutineId routineId,
        Comment? comment = null)
        : base(id)
    {
        Duration = duration;
        EndDateTime = endDateTime;
        RestTimeBetweenExercises = restTimeBetweenExercises;
        UserId = userId;
        RoutineId = routineId;
        Comment = comment;
    }

    internal static Workout Create(
        WorkoutId id,
        TimeSpan duration,
        DateTime endDateTime,
        TimeSpan restTimeBetweenExercises,
        UserId userId,
        RoutineId routineId,
        Comment? comment = null)
        => new Workout(id, duration, endDateTime, restTimeBetweenExercises, userId, routineId, comment);
}
