namespace WorkoutTracker.Domain.Routines;

using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Routines.ValueObjects;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public class Routine : AggregateRoot<RoutineId>
{
    private readonly List<RoutineExercise> _routineExercises = [];

    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public UserId UserId { get; private set; }

    public IReadOnlyCollection<RoutineExercise> RoutineExercises => _routineExercises.AsReadOnly();

    private Routine()
    {
        Name = null!;
        Description = null!;
        UserId = null!;
    }

    private Routine(
        RoutineId id,
        Name name,
        Description description,
        UserId userId)
        : base(id)
    {
        Name = name;
        Description = description;
        UserId = userId;
    }

    public static Result<Routine> Create(
        Name name,
        Description description,
        UserId userId)
    {
        Result<RoutineId> routineIdResult = RoutineId.New();

        return Result.Combine(
            Name.EnsureNotNull(name),
            Description.EnsureNotNull(description),
            UserId.EnsureNotNull(userId),
            routineIdResult)
            .OnSuccess(() => new Routine(
                routineIdResult.ValueOrDefault(),
                name,
                description,
                userId));
    }

    public Result<Routine> UpdateName(Name newName)
    {
        return Name.EnsureNotNull(newName)
            .OnSuccess(n =>
            {
                if (Name != n)
                    Name = n;
            })
            .Map(_ => this);
    }

    public Result<Routine> UpdateDescription(Description newDescription)
    {
        return Description.EnsureNotNull(newDescription)
            .OnSuccess(d =>
            {
                if (Description != d)
                    Description = d;
            })
            .Map(_ => this);
    }

    public Result<Routine> ReassignToUser(UserId newUserId)
    {

        return UserId.EnsureNotNull(newUserId)
            .OnSuccess(uId =>
            {
                if (UserId != uId)
                    UserId = uId;
            })
            .Map(_ => this);
    }

    public Result<RoutineExercise> AddExercise(
        byte setCount,
        byte repCount,
        TimeSpan restTimeBetweenSets,
        Comment comment,
        ExercisePosition position,
        ExerciseId exerciseId)
    {
        return Result.Combine(
            Comment.EnsureNotNull(comment),
            ExercisePosition.EnsureNotNull(position),
            ExerciseId.EnsureNotNull(exerciseId))
            .OnSuccess(() => RoutineExercise.Create(
            setCount,
            repCount,
            restTimeBetweenSets,
            comment,
            position,
            Id,
            exerciseId))
            .OnSuccess(re => _routineExercises.Add(re));
    }

    public Result<Routine> RemoveExercise(RoutineExerciseId routineExerciseid)
    {
        return RoutineExerciseId.EnsureNotNull(routineExerciseid)
            .Map(reId => _routineExercises.Find(re => re.Id == reId))
            .Ensure(re => re is not null, DomainErrors.RoutineExercise.NotFound)
            .Ensure(re => _routineExercises.Remove(re!), DomainErrors.RoutineExercise.CannotRemove)
            .Map(_ => this);
    }
}
