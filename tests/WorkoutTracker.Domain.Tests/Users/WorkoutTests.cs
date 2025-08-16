namespace WorkoutTracker.Domain.Tests.Users;

using FluentAssertions;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class WorkoutTests
{
    [Fact]
    public void UpdateStartTime_Should_ReturnSuccess_When_StartTimeIsEarlierThanEndTime()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("password").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();
        Workout workout = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault())
            .ValueOrDefault();
        DateTime newStartDate = DateTime.MinValue;

        // Act
        Result<Workout> workoutResult = workout.UpdateStartTime(newStartDate);

        // Assert
        workoutResult.IsSuccess.Should().BeTrue();
        workoutResult.ValueOrDefault().Should().NotBeNull();
        workoutResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        workout.StartTime.Should().Be(newStartDate);
    }

    [Fact]
    public void UpdateStartTime_Should_ReturnFailure_When_StartTimeIsLaterThanEndTime()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("password").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();
        Workout workout = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault())
            .ValueOrDefault();
        DateTime newStartDate = DateTime.MaxValue;

        // Act
        Result<Workout> workoutResult = workout.UpdateStartTime(newStartDate);

        // Assert
        workoutResult.IsFailure.Should().BeTrue();
        workoutResult.ValueOrDefault().Should().BeNull();
        workoutResult.Errors.Should().Contain(DomainErrors.Workout.InvalidStartEndTime);
        workout.EndTime.Should().NotBe(newStartDate);
    }

    [Fact]
    public void UpdateEndTime_Should_ReturnSuccess_When_EndTimeIsLaterThanStartTime()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("password").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();
        Workout workout = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault())
            .ValueOrDefault();
        DateTime newEndDate = DateTime.MaxValue;

        // Act
        Result<Workout> workoutResult = workout.UpdateEndTime(newEndDate);

        // Assert
        workoutResult.IsSuccess.Should().BeTrue();
        workoutResult.ValueOrDefault().Should().NotBeNull();
        workoutResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        workout.EndTime.Should().Be(newEndDate);
    }

    [Fact]
    public void UpdateEndTime_Should_ReturnFailure_When_EndTimeIsEarlierThanStartTime()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("password").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();
        Workout workout = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault())
            .ValueOrDefault();
        DateTime newEndDate = DateTime.MinValue;

        // Act
        Result<Workout> workoutResult = workout.UpdateEndTime(newEndDate);

        // Assert
        workoutResult.IsFailure.Should().BeTrue();
        workoutResult.ValueOrDefault().Should().BeNull();
        workoutResult.Errors.Should().Contain(DomainErrors.Workout.InvalidStartEndTime);
        workout.EndTime.Should().NotBe(newEndDate);
    }

    [Fact]
    public void UpdateComment_Should_ReturnSuccess_When_CommentIsValid()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("password").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();
        Workout workout = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault())
            .ValueOrDefault();
        Comment comment = Comment.Create("This is a comment.").ValueOrDefault();

        // Act
        Result<Workout> workoutResult = workout.UpdateComment(comment);

        // Assert
        workoutResult.IsSuccess.Should().BeTrue();
        workoutResult.ValueOrDefault().Should().NotBeNull();
        workoutResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        workout.Comment.Should().Be(comment);
    }

    [Fact]
    public void UpdateComment_Should_ReturnFailure_When_CommentIsNull()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("password").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();
        Workout workout = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault())
            .ValueOrDefault();
        Comment? comment = null;

        // Act
        Result<Workout> workoutResult = workout.UpdateComment(comment!);

        // Assert
        workoutResult.IsFailure.Should().BeTrue();
        workoutResult.ValueOrDefault().Should().BeNull();
        workoutResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Comment.Null);
        workout.Comment.Should().NotBe(comment);
    }

    [Fact]
    public void WorkoutsWithSameValues_Should_NotBeEqual()
    {
        // Arrange
        User user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("password").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            Gender.Male,
            UserRole.User,
            DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();
        Workout workout1 = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault())
            .ValueOrDefault();
        Workout workout2 = user.AddWorkout(
            DateTime.UtcNow,
            DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault())
            .ValueOrDefault();

        // Act
        bool workoutsAreDifferent = workout1 != workout2;

        // Assert
        workoutsAreDifferent.Should().BeTrue();
    }
}
