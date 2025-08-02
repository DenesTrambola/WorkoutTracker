namespace WorkoutTracker.Domain.Exercises;

using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public class Exercise : AggregateRoot<ExerciseId>
{
    public Name Name { get; private set; }
    public TargetMuscle TargetMuscle { get; private set; }
    public Visibility IsPublic { get; private set; }
    public UserId UserId { get; private set; }

    private Exercise(
        ExerciseId id,
        Name name,
        TargetMuscle targetMuscle,
        Visibility visibility,
        UserId userId)
        : base(id)
    {
        Name = name;
        TargetMuscle = targetMuscle;
        IsPublic = visibility;
        UserId = userId;
    }

    public static Exercise Create(
        ExerciseId id,
        Name name,
        TargetMuscle targetMuscle,
        Visibility visibility,
        UserId userId)
        => new Exercise(id, name, targetMuscle, visibility, userId);
}
