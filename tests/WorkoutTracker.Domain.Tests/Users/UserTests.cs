namespace WorkoutTracker.Domain.Tests.Users;

using System.Data;
using FluentAssertions;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class UserTests
{
    [Fact]
    public void Create_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Act
        Result<User> userResult = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1));

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_UsernameIsNull()
    {
        // Arrange
        Username? username = null;

        // Act
        Result<User> userResult = User.Create(
            username!,
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1));

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Username.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_PasswordHashIsNull()
    {
        // Arrange
        PasswordHash? passwordHash = null;

        // Act
        Result<User> userResult = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            passwordHash!,
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1));

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.PasswordHash.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_EmailIsNull()
    {
        // Arrange
        Email? email = null;

        // Act
        Result<User> userResult = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            email!,
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1));

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Email.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_FullNameIsNull()
    {
        // Arrange
        FullName? fullName = null;

        // Act
        Result<User> userResult = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            fullName!,
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1));

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.FullName.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_GenderIsInvalid()
    {
        // Arrange
        Gender gender = (Gender)10;

        // Act
        Result<User> userResult = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            gender,
            UserRole.User,
            new DateOnly(2000, 1, 1));

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Gender.Invalid);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_RoleIsInvalid()
    {
        // Arrange
        UserRole role = (UserRole)10;

        // Act
        Result<User> userResult = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            role,
            new DateOnly(2000, 1, 1));

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.UserRole.Invalid);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_BirthDateIsInvalid()
    {
        // Arrange
        DateOnly birthDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);

        // Act
        Result<User> userResult = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            birthDate);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.BirthDate.Invalid);
    }

    [Fact]
    public void UpdateUsername_Should_ReturnSuccess_When_UsernameIsValid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        Username newUsername = Username.Create("deinesh").ValueOrDefault();

        // Act
        Result<User> userResult = user.UpdateUsername(newUsername);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().Username.Should().Be(newUsername);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateUsername_Should_ReturnFailure_When_UsernameIsNull()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        Username? newUsername = null;

        // Act
        Result<User> userResult = user.UpdateUsername(newUsername!);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Username.Null);
    }

    [Fact]
    public void UpdatePasswordHash_Should_ReturnSuccess_When_PasswordIsValid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        PasswordHash newPasswordHash = PasswordHash.Create("password").ValueOrDefault();

        // Act
        Result<User> userResult = user.UpdatePasswordHash(newPasswordHash);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().PasswordHash.Should().Be(newPasswordHash);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdatePasswordHash_Should_ReturnFailure_When_PasswordIsNull()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        PasswordHash? newPasswordHash = null;

        // Act
        Result<User> userResult = user.UpdatePasswordHash(newPasswordHash!);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.PasswordHash.Null);
    }

    [Fact]
    public void UpdateEmail_Should_ReturnSuccess_When_EmailIsValid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        Email newEmail = Email.Create("deinesh@gmail.com").ValueOrDefault();

        // Act
        Result<User> userResult = user.UpdateEmail(newEmail);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().Email.Should().Be(newEmail);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateEmail_Should_ReturnFailure_When_EmailIsNull()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        Email? newEmail = null;

        // Act
        Result<User> userResult = user.UpdateEmail(newEmail!);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Email.Null);
    }

    [Fact]
    public void UpdateFullName_Should_ReturnSuccess_When_FullNameIsValid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        FullName newFullName = FullName.Create("First", "Last").ValueOrDefault();

        // Act
        Result<User> userResult = user.UpdateFullName(newFullName);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().FullName.Should().Be(newFullName);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateFullName_Should_ReturnFailure_When_FullNameIsNull()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        FullName? newFullName = null;

        // Act
        Result<User> userResult = user.UpdateFullName(newFullName!);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.FullName.Null);
    }

    [Fact]
    public void UpdateGender_Should_ReturnSuccess_When_GenderIsValid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        Gender newGender = Gender.Female;

        // Act
        Result<User> userResult = user.UpdateGender(newGender);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().Gender.Should().Be(newGender);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateGender_Should_ReturnFailure_When_GenderIsInvalid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        Gender newGender = (Gender)10;

        // Act
        Result<User> userResult = user.UpdateGender(newGender);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Gender.Invalid);
    }

    [Fact]
    public void UpdateRole_Should_ReturnSuccess_When_RoleIsValid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        UserRole newRole = UserRole.Admin;

        // Act
        Result<User> userResult = user.UpdateRole(newRole);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().Role.Should().Be(newRole);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateRole_Should_ReturnFailure_When_RoleIsInvalid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        UserRole newRole = (UserRole)10;

        // Act
        Result<User> userResult = user.UpdateRole(newRole);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.UserRole.Invalid);
    }

    [Fact]
    public void UpdateBirthDate_Should_ReturnSuccess_When_BirthDateIsValid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        DateOnly newBirthDate = new DateOnly(2025, 1, 1);

        // Act
        Result<User> userResult = user.UpdateBirthDate(newBirthDate);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().BirthDate.Should().Be(newBirthDate);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateBirthDate_Should_ReturnFailure_When_BirthDateIsInvalid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        DateOnly newBirthDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);

        // Act
        Result<User> userResult = user.UpdateBirthDate(newBirthDate);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.BirthDate.Invalid);
    }

    [Fact]
    public void AddWorkout_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();

        // Act
        Result<Workout> workoutResult = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault());

        // Assert
        workoutResult.IsSuccess.Should().BeTrue();
        workoutResult.ValueOrDefault().Should().NotBeNull();
        workoutResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        user.Workouts.Should().Contain(workoutResult.ValueOrDefault());
    }

    [Fact]
    public void AddWorkout_Should_ReturnFailure_When_CommentIsNull()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        Comment? comment = null;

        // Act
        Result<Workout> workoutResult = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            comment!,
            RoutineId.New().ValueOrDefault());

        // Assert
        workoutResult.IsFailure.Should().BeTrue();
        workoutResult.ValueOrDefault().Should().BeNull();
        workoutResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Comment.Null);
        user.Workouts.Should().NotContain(workoutResult.ValueOrDefault());
    }

    [Fact]
    public void AddWorkout_Should_ReturnFailure_When_RoutineIdIsNull()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        RoutineId? routineId = null;

        // Act
        Result<Workout> workoutResult = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            routineId!);

        // Assert
        workoutResult.IsFailure.Should().BeTrue();
        workoutResult.ValueOrDefault().Should().BeNull();
        workoutResult.Errors.Should().Contain(Domain.Routines.Errors.DomainErrors.RoutineId.Null);
        user.Workouts.Should().NotContain(workoutResult.ValueOrDefault());
    }

    [Fact]
    public void AddWorkout_Should_ReturnFailure_When_StartTimeIsLaterThanEndTime()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        DateTime startTime = DateTime.UtcNow.AddDays(1);
        DateTime endTime = DateTime.UtcNow;

        // Act
        Result<Workout> workoutResult = user.AddWorkout(
            startTime,
            endTime,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault());

        // Assert
        workoutResult.IsFailure.Should().BeTrue();
        workoutResult.ValueOrDefault().Should().BeNull();
        workoutResult.Errors.Should().Contain(DomainErrors.Workout.InvalidStartEndTime);
        user.Workouts.Should().NotContain(workoutResult.ValueOrDefault());
    }

    [Fact]
    public void AddWorkout_Should_ReturnFailure_When_RestTimeIsInvalid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        TimeSpan restTimeBetweenSets = new TimeSpan(0, 0, -1);

        // Act
        Result<Workout> workoutResult = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            restTimeBetweenSets,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault());

        // Assert
        workoutResult.IsFailure.Should().BeTrue();
        workoutResult.ValueOrDefault().Should().BeNull();
        workoutResult.Errors.Should().Contain(DomainErrors.Workout.InvalidRestTime);
        user.Workouts.Should().NotContain(workoutResult.ValueOrDefault());
    }

    [Fact]
    public void RemoveWorkout_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        Workout workout = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault())
            .ValueOrDefault();

        // Act
        Result<User> userResult = user.RemoveWorkout(workout.Id);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        user.Workouts.Should().NotContain(workout);
    }

    [Fact]
    public void RemoveWorkout_Should_ReturnFailure_When_WorkoutIdIsNull()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        WorkoutId? workoutId = null;

        // Act
        Result<User> userResult = user.RemoveWorkout(workoutId!);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.WorkoutId.Null);
    }

    [Fact]
    public void RemoveWorkout_Should_ReturnFailure_When_WorkoutIsNotFound()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        Workout workout = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault())
            .ValueOrDefault();
        Result<User> userResult = user.RemoveWorkout(workout.Id);

        // Act
        userResult = user.RemoveWorkout(workout.Id);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Workout.NotFound);
        user.Workouts.Should().NotContain(workout);
    }

    [Fact]
    public void UsersWithSameValues_ShouldNot_BeEqual()
    {
        // Arrange
        User user1 = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();
        User user2 = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("123").ValueOrDefault(),
            Email.Create("example@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            new DateOnly(2000, 1, 1))
            .ValueOrDefault();

        // Act
        bool usersAreDifferent = user1 != user2;

        // Assert
        usersAreDifferent.Should().BeTrue();
    }
}
