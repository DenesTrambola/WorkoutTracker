namespace WorkoutTracker.Domain.Exercises;

using WorkoutTracker.Domain.Exercises.Errors;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public class Exercise : AggregateRoot<ExerciseId>
{
    public Name Name { get; private set; }
    public TargetMuscle TargetMuscle { get; private set; }
    public Visibility Visibility { get; private set; }
    public UserId UserId { get; private set; }

    private Exercise()
    {
        Name = null!;
        TargetMuscle = null!;
        Visibility = null!;
        UserId = null!;
    }

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
        Visibility = visibility;
        UserId = userId;
    }

    public static Result<Exercise> Create(
        Name name,
        TargetMuscle targetMuscle,
        Visibility visibility,
        UserId userId)
    {
        Result<ExerciseId> workoutIdResult = ExerciseId.New();

        return Result.Combine(
            Name.EnsureNotNull(name),
            TargetMuscle.EnsureNotNull(targetMuscle),
            Visibility.EnsureNotNull(visibility),
            UserId.EnsureNotNull(userId),
            workoutIdResult)
            .OnSuccess(() => new Exercise(
                workoutIdResult.ValueOrDefault(),
                name,
                targetMuscle,
                visibility,
                userId));
    }

    public Result<Exercise> UpdateName(Name newName)
    {
        Name = newName;
        return this;
    }

    public Result<Exercise> UpdateTargetMuscle(TargetMuscle newTargetMuscle)
    {
        TargetMuscle = newTargetMuscle;
        return this;
    }

    public Result<Exercise> UpdateVisibility(Visibility newVisibility)
    {
        Visibility = newVisibility;
        return this;
    }
}
