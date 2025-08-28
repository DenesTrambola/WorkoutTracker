namespace WorkoutTracker.Domain.Tests.Routines;

using FluentAssertions;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Routines.ValueObjects;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class RoutineExerciseTests
{
    private readonly Routine _routine = Routine.Create(
        Name.Create("Push").ValueOrDefault(),
        Description.Create(null).ValueOrDefault(),
        UserId.New().ValueOrDefault())
        .ValueOrDefault();

    [Fact]
    public void UpdateSetCound_Should_ReturnSuccess_When_SetCountIsValid()
    {
        // Arrange
        RoutineExercise exercise = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        byte newSetCount = 4;

        // Act
        Result<RoutineExercise> exerciseResult = exercise.UpdateSetCount(newSetCount);

        // Assert
        exerciseResult.IsSuccess.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().NotBeNull();
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        exercise.SetCount.Should().Be(newSetCount);
    }

    [Fact]
    public void UpdateSetCount_Should_ReturnFailure_When_SetCountIsInvalid()
    {
        // Arrange
        RoutineExercise exercise = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        byte newSetCount = 0;

        // Act
        Result<RoutineExercise> exerciseResult = exercise.UpdateSetCount(newSetCount);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.RoutineExercise.InvalidSetCount);
        exercise.SetCount.Should().NotBe(newSetCount);
    }

    [Fact]
    public void UpdateRepCount_Should_ReturnSuccess_When_RepCountIsValid()
    {
        // Arrange
        RoutineExercise exercise = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        byte newRepCount = 10;

        // Act
        Result<RoutineExercise> exerciseResult = exercise.UpdateRepCount(newRepCount);

        // Assert
        exerciseResult.IsSuccess.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().NotBeNull();
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        exercise.RepCount.Should().Be(newRepCount);
    }

    [Fact]
    public void UpdateRepCount_Should_ReturnFailure_When_RepCountIsInvalid()
    {
        // Arrange
        RoutineExercise exercise = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        byte newRepCount = 0;

        // Act
        Result<RoutineExercise> exerciseResult = exercise.UpdateRepCount(newRepCount);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.RoutineExercise.InvalidRepCount);
        exercise.SetCount.Should().NotBe(newRepCount);
    }

    [Fact]
    public void UpdateRestTime_Should_ReturnSuccess_When_RestTimeIsValid()
    {
        // Arrange
        RoutineExercise exercise = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        TimeSpan newRestTime = TimeSpan.MaxValue;

        // Act
        Result<RoutineExercise> exerciseResult = exercise.UpdateRestTimeBetweenSets(newRestTime);

        // Assert
        exerciseResult.IsSuccess.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().NotBeNull();
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        exercise.RestTimeBetweenSets.Should().Be(newRestTime);
    }

    [Fact]
    public void UpdateRestTime_Should_ReturnFailure_When_RestTimeIsInvalid()
    {
        // Arrange
        RoutineExercise exercise = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        TimeSpan newRestTime = TimeSpan.MinValue;

        // Act
        Result<RoutineExercise> exerciseResult = exercise.UpdateRestTimeBetweenSets(newRestTime);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.RoutineExercise.InvalidRestTimeBetweenSets);
        exercise.RestTimeBetweenSets.Should().NotBe(newRestTime);
    }

    [Fact]
    public void UpdateComment_Should_ReturnSuccess_When_CommentIsNotNull()
    {
        // Arrange
        RoutineExercise exercise = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        Comment newComment = Comment.Create("This is a comment.").ValueOrDefault();

        // Act
        Result<RoutineExercise> exerciseResult = exercise.UpdateComment(newComment!);

        // Assert
        exerciseResult.IsSuccess.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().NotBeNull();
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        exercise.Comment.Should().Be(newComment);
    }

    [Fact]
    public void UpdateComment_Should_ReturnFailure_When_CommentIsNull()
    {
        // Arrange
        RoutineExercise exercise = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        Comment? newComment = null;

        // Act
        Result<RoutineExercise> exerciseResult = exercise.UpdateComment(newComment!);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Comment.Null);
        exercise.Comment.Should().NotBe(newComment);
    }

    [Fact]
    public void UpdatePosition_Should_ReturnSuccess_When_PositionIsNotNull()
    {
        // Arrange
        RoutineExercise exercise = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        ExercisePosition newPosition = ExercisePosition.Create(2).ValueOrDefault();

        // Act
        Result<RoutineExercise> exerciseResult = exercise.UpdatePosition(newPosition);

        // Assert
        exerciseResult.IsSuccess.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().NotBeNull();
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        exercise.Position.Should().Be(newPosition);
    }

    [Fact]
    public void UpdatePosition_Should_ReturnFailure_When_PositionIsNull()
    {
        // Arrange
        RoutineExercise exercise = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        ExercisePosition? newPosition = null;

        // Act
        Result<RoutineExercise> exerciseResult = exercise.UpdatePosition(newPosition!);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.ExercisePosition.Null);
        exercise.Position.Should().NotBe(newPosition);
    }

    [Fact]
    public void RoutineExercisesWithSameValues_Should_NotBeEqual()
    {
        // Arrange
        RoutineExercise exercise1 = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        RoutineExercise exercise2 = _routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();

        // Act
        bool exercisesAreDifferent = exercise1 != exercise2;

        // Assert
        exercisesAreDifferent.Should().BeTrue();
    }
}
