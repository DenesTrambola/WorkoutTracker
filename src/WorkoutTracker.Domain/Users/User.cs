namespace WorkoutTracker.Domain.Users;

using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Measurement;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public class User : AggregateRoot<UserId>
{
    private readonly List<Routine> _routines = new();
    private readonly List<Workout> _workouts = new();
    private readonly List<Exercise> _exercises = new();
    private readonly List<Measurement> _measurements = new();

    public Username Username { get; private set; }
    public PasswordHash PasswordHash { get; private set; }
    public Email Email { get; private set; }
    public FullName FullName { get; private set; }
    public Gender Gender { get; private set; }
    public UserRole Role { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public IReadOnlyCollection<Routine> Routines => _routines.AsReadOnly();
    public IReadOnlyCollection<Workout> Workouts => _workouts.AsReadOnly();
    public IReadOnlyCollection<Exercise> Exercises => _exercises.AsReadOnly();
    public IReadOnlyCollection<Measurement> Measurements => _measurements.AsReadOnly();

    private User(
        UserId id,
        Username username,
        PasswordHash passwordHash,
        Email email,
        FullName fullName,
        Gender gender,
        UserRole role,
        DateOnly birthDate,
        DateTime createdOn)
        : base(id)
    {
        Username = username;
        PasswordHash = passwordHash;
        Email = email;
        FullName = fullName;
        Gender = gender;
        Role = role;
        BirthDate = birthDate;
        CreatedOn = createdOn;
    }

    public static Result<User> Create(
        UserId id,
        Username username,
        PasswordHash passwordHash,
        Email email,
        FullName fullName,
        Gender gender,
        UserRole role,
        DateOnly birthDate,
        DateTime createdOn)
        => new User(id, username, passwordHash, email, fullName, gender, role, birthDate, createdOn);
}
