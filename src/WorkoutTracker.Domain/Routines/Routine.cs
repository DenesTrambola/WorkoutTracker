namespace WorkoutTracker.Domain.Routines;

using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public class Routine : AggregateRoot<RoutineId>
{
    private readonly List<RoutineExercise> _exercises = new();

    public Name Name { get; private set; }
    public Description? Description { get; private set; }
    public UserId UserId { get; private set; }

    public IReadOnlyCollection<RoutineExercise> Exercises => _exercises.AsReadOnly();

    private Routine(
        RoutineId id,
        Name name,
        Description? description,
        UserId userId)
        : base(id)
    {
        Name = name;
        Description = description;
        UserId = userId;
    }

    public static Routine Create(
        RoutineId id,
        Name name,
        Description? description,
        UserId userId)
        => new Routine(id, name, description, userId);
}
