namespace WorkoutTracker.Domain.Users;

using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public class User : AggregateRoot<UserId>
{
    private readonly List<RoutineId> _routineIds = [];
    private readonly List<Workout> _workouts = [];
    private readonly List<ExerciseId> _exerciseIds = [];
    private readonly List<MeasurementId> _measurementIds = [];

    public Username Username { get; private set; }
    public PasswordHash PasswordHash { get; private set; }
    public Email Email { get; private set; }
    public FullName FullName { get; private set; }
    public Gender Gender { get; private set; }
    public UserRole Role { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public IReadOnlyCollection<RoutineId> RoutineIds => _routineIds.AsReadOnly();
    public IReadOnlyCollection<Workout> Workouts => _workouts.AsReadOnly();
    public IReadOnlyCollection<ExerciseId> ExerciseIds => _exerciseIds.AsReadOnly();
    public IReadOnlyCollection<MeasurementId> MeasurementIds => _measurementIds.AsReadOnly();

    private User()
    {
        Username = null!;
        PasswordHash = null!;
        Email = null!;
        FullName = null!;
    }

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
        Username username,
        PasswordHash passwordHash,
        Email email,
        FullName fullName,
        Gender gender,
        UserRole role,
        DateOnly birthDate)
    {
        Result<UserId> userIdResult = UserId.New();

        return Result.Combine(
            Username.EnsureNotNull(username),
            PasswordHash.EnsureNotNull(passwordHash),
            Email.EnsureNotNull(email),
            FullName.EnsureNotNull(fullName),
            EnsureGenderIsDefined(gender),
            EnsureRoleIsDefined(role),
            EnsureBirthDateIsValid(birthDate),
            userIdResult)
            .OnSuccess(() => new User(
                userIdResult.ValueOrDefault(),
                username,
                passwordHash,
                email,
                fullName,
                gender,
                role,
                birthDate,
                DateTime.UtcNow));
    }

    private static Result<Gender> EnsureGenderIsDefined(Gender gender)
    {
        return Result.Ensure(
            gender,
            g => Enum.IsDefined(g),
            DomainErrors.Gender.InvalidValue);
    }

    private static Result<UserRole> EnsureRoleIsDefined(UserRole role)
    {
        return Result.Ensure(
            role,
            r => Enum.IsDefined(r),
            DomainErrors.UserRole.InvalidValue);
    }

    private static Result<DateOnly> EnsureBirthDateIsValid(DateOnly birthDate)
    {
        return Result.Ensure(
            birthDate,
            bd => bd <= DateOnly.FromDateTime(DateTime.UtcNow),
            DomainErrors.BirthDate.InvalidValue);
    }

    public Result<User> UpdateUsername(Username newUsername)
    {
        return Result.Ensure(
            newUsername,
            un => un is not null,
            DomainErrors.Username.Null)
            .OnSuccess(un => Username = un)
            .Map(_ => this);
    }

    public Result<User> UpdatePasswordHash(PasswordHash newPasswordHash)
    {
        return Result.Ensure(
            newPasswordHash,
            ph => ph is not null,
            DomainErrors.PasswordHash.Null)
            .OnSuccess(ph => PasswordHash = ph)
            .Map(_ => this);
    }

    public Result<User> UpdateEmail(Email email)
    {
        return Result.Ensure(
            email,
            em => em is not null,
            DomainErrors.Email.Null)
            .OnSuccess(em => Email = em)
            .Map(_ => this);
    }

    public Result<User> UpdateFullName(FullName newFullName)
    {
        return Result.Ensure(
            newFullName,
            fn => fn is not null,
            DomainErrors.FullName.Null)
            .OnSuccess(fn => FullName = fn)
            .Map(_ => this);
    }

    public Result<User> UpdateGender(Gender gender)
    {
        return Result.Ensure(
            gender,
            g => Enum.IsDefined(g),
            DomainErrors.Gender.InvalidValue)
            .OnSuccess(g => Gender = gender)
            .Map(_ => this);
    }

    public Result<User> UpdateRole(UserRole role)
    {
        return Result.Ensure(
            role,
            r => Enum.IsDefined(r),
            DomainErrors.UserRole.InvalidValue)
            .OnSuccess(r => Role = role)
            .Map(_ => this);
    }

    public Result<User> UpdateBirthDate(DateOnly birthDate)
    {
        return Result.Ensure(
            birthDate,
            bd => bd != default,
            DomainErrors.BirthDate.InvalidValue)
            .OnSuccess(bd => BirthDate = bd)
            .Map(_ => this);
    }
}
