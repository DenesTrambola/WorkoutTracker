namespace WorkoutTracker.Domain.Exercises;

using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.TypedIds;

public class Exercise : AggregateRoot<ExerciseId>
{
    public Name Name { get; private set; }
    public TargetMuscle TargetMuscle { get; private set; }
    public Visibility Visibility { get; private set; }
    public UserId UserId { get; private set; }

    public User User { get; private set; }

    private Exercise()
    {
        Name = null!;
        TargetMuscle = null!;
        Visibility = null!;
        UserId = null!;
        User = null!;
    }

    private Exercise(
        ExerciseId id,
        Name name,
        TargetMuscle targetMuscle,
        Visibility visibility,
        User user)
        : base(id)
    {
        Name = name;
        TargetMuscle = targetMuscle;
        Visibility = visibility;
        User = user;
        UserId = user.Id;
    }

    public static Result<Exercise> Create(
        ExerciseId id,
        Name name,
        TargetMuscle targetMuscle,
        Visibility visibility,
        User user)
    {
        return Result.Ensure(
            user,
            u => u is not null,
            DomainErrors.User.Null)
            .Map(u => new Exercise(id, name, targetMuscle, visibility, u));
    }
}
