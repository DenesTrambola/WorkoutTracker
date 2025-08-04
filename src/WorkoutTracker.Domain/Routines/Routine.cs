namespace WorkoutTracker.Domain.Routines;

using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.TypedIds;

public class Routine : AggregateRoot<RoutineId>
{
    private readonly List<RoutineExercise> _exercises = [];

    public Name Name { get; private set; }
    public Description? Description { get; private set; }
    public UserId UserId { get; private set; }

    public User User { get; private set; }

    public IReadOnlyCollection<RoutineExercise> Exercises => _exercises.AsReadOnly();

    private Routine()
    {
        Name = null!;
        UserId = null!;
        User = null!;
    }

    private Routine(
        RoutineId id,
        Name name,
        Description? description,
        User user)
        : base(id)
    {
        Name = name;
        Description = description;
        User = user;
        UserId = user.Id;
    }

    public static Result<Routine> Create(
        RoutineId id,
        Name name,
        Description? description,
        User user)
    {
        return Result.Ensure(
            user,
            u => u is not null,
            DomainErrors.User.Null)
            .Map(u => new Routine(id, name, description, u));
    }
}
