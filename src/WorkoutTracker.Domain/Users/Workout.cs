namespace WorkoutTracker.Domain.Users;

using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public class Workout : Entity<WorkoutId>
{
    public TimeSpan Duration { get; private set; }
    public DateTime EndTime { get; private set; }
    public TimeSpan RestTimeBetweenExercises { get; private set; }
    public Comment Comment { get; private set; }
    public UserId UserId { get; private set; }
    public RoutineId RoutineId { get; private set; }

    public User User { get; private set; }
    public Routine Routine { get; private set; }

    private Workout()
    {
        Comment = null!;
        UserId = null!;
        RoutineId = null!;
        User = null!;
        Routine = null!;
    }

    private Workout(
        WorkoutId id,
        TimeSpan duration,
        DateTime endDateTime,
        TimeSpan restTimeBetweenExercises,
        Comment comment,
        User user,
        Routine routine)
        : base(id)
    {
        Duration = duration;
        EndTime = endDateTime;
        RestTimeBetweenExercises = restTimeBetweenExercises;
        User = user;
        Routine = routine;
        UserId = user.Id;
        RoutineId = routine.Id;
        Comment = comment;
    }

    internal static Result<Workout> Create(
        WorkoutId id,
        TimeSpan duration,
        DateTime endDateTime,
        TimeSpan restTimeBetweenExercises,
        Comment comment,
        User user,
        Routine routine)
    {
        return new Workout(id, duration, endDateTime, restTimeBetweenExercises, comment, user, routine);
    }
}
