namespace WorkoutTracker.Domain.Tests.Exercises;

using FluentAssertions;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Exercises.Errors;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class ExerciseTests
{
    private readonly Name _validName = Name.Create("Pull Up").ValueOrDefault();
    private readonly TargetMuscle _validTargetMuscle = TargetMuscle.Create("Back").ValueOrDefault();
    private readonly Visibility _validVisibility = Visibility.Create(false).ValueOrDefault();
    private readonly UserId _validUserId = UserId.New().ValueOrDefault();

    [Fact]
    public void Create_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Act
        Result<Exercise> exerciseResult = Exercise.Create(
            _validName,
            _validTargetMuscle,
            _validVisibility,
            _validUserId);

        // Assert
        exerciseResult.IsSuccess.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().NotBeNull();
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_NameIsNull()
    {
        // Arrange
        Name? name = null;

        // Act
        Result<Exercise> exerciseResult = Exercise.Create(
            name!,
            _validTargetMuscle,
            _validVisibility,
            _validUserId);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Name.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_TargetMuscleIsNull()
    {
        // Arrange
        TargetMuscle? targetMuscle = null;

        // Act
        Result<Exercise> exerciseResult = Exercise.Create(
            _validName,
            targetMuscle!,
            _validVisibility,
            _validUserId);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.TargetMuscle.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_VisibilityIsNull()
    {
        // Arrange
        Visibility? visibility = null;

        // Act
        Result<Exercise> exerciseResult = Exercise.Create(
            _validName,
            _validTargetMuscle,
            visibility!,
            _validUserId);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.Visibility.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_UserIdIsNull()
    {
        // Arrange
        UserId? userId = null;

        // Act
        Result<Exercise> exerciseResult = Exercise.Create(
            _validName,
            _validTargetMuscle,
            _validVisibility,
            userId!);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.UserId.Null);
    }

    [Fact]
    public void UpdateName_Should_ReturnSuccess_When_NameIsValid()
    {
        // Arrange
        Exercise exercise = Exercise.Create(
            _validName,
            _validTargetMuscle,
            _validVisibility,
            _validUserId)
            .ValueOrDefault();
        Name newName = Name.Create("Inverted Row").ValueOrDefault();

        // Act
        Result<Exercise> exerciseResult = exercise.UpdateName(newName);

        // Assert
        exerciseResult.IsSuccess.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().NotBeNull();
        exerciseResult.ValueOrDefault().Name.Should().Be(newName);
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateName_Should_ReturnFailure_When_NameIsNull()
    {
        // Arrange
        Exercise exercise = Exercise.Create(
            _validName,
            _validTargetMuscle,
            _validVisibility,
            _validUserId)
            .ValueOrDefault();
        Name? newName = null;

        // Act
        Result<Exercise> exerciseResult = exercise.UpdateName(newName!);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Name.Null);
    }

    [Fact]
    public void UpdateTargetMuscle_Should_ReturnSuccess_When_TargetMuscleIsValid()
    {
        // Arrange
        Exercise exercise = Exercise.Create(
            _validName,
            _validTargetMuscle,
            _validVisibility,
            _validUserId)
            .ValueOrDefault();
        TargetMuscle newTargetMuscle = TargetMuscle.Create("Biceps").ValueOrDefault();

        // Act
        Result<Exercise> exerciseResult = exercise.UpdateTargetMuscle(newTargetMuscle);

        // Assert
        exerciseResult.IsSuccess.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().NotBeNull();
        exerciseResult.ValueOrDefault().TargetMuscle.Should().Be(newTargetMuscle);
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateName_Should_ReturnFailure_When_TargetMuscleIsNull()
    {
        // Arrange
        Exercise exercise = Exercise.Create(
            _validName,
            _validTargetMuscle,
            _validVisibility,
            _validUserId)
            .ValueOrDefault();
        TargetMuscle? newTargetMuscle = null;

        // Act
        Result<Exercise> exerciseResult = exercise.UpdateTargetMuscle(newTargetMuscle!);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.TargetMuscle.Null);
    }

    [Fact]
    public void UpdateTargetMuscle_Should_ReturnSuccess_When_VisibilityIsValid()
    {
        // Arrange
        Exercise exercise = Exercise.Create(
            _validName,
            _validTargetMuscle,
            _validVisibility,
            _validUserId)
            .ValueOrDefault();
        Visibility newVisibility = Visibility.Create(true).ValueOrDefault();

        // Act
        Result<Exercise> exerciseResult = exercise.UpdateVisibility(newVisibility);

        // Assert
        exerciseResult.IsSuccess.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().NotBeNull();
        exerciseResult.ValueOrDefault().Visibility.Should().Be(newVisibility);
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateVisibility_Should_ReturnFailure_When_VisibilityIsNull()
    {
        // Arrange
        Exercise exercise = Exercise.Create(
            _validName,
            _validTargetMuscle,
            _validVisibility,
            _validUserId)
            .ValueOrDefault();
        Visibility? newVisibility = null;

        // Act
        Result<Exercise> exerciseResult = exercise.UpdateVisibility(newVisibility!);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.Visibility.Null);
    }

    [Fact]
    public void ExercisesWithSameValues_ShouldNot_BeEqual()
    {
        // Arrange
        Exercise exercise1 = Exercise.Create(
            _validName,
            _validTargetMuscle,
            _validVisibility,
            _validUserId)
            .ValueOrDefault();
        Exercise exercise2 = Exercise.Create(
            _validName,
            _validTargetMuscle,
            _validVisibility,
            _validUserId)
            .ValueOrDefault();

        // Act
        bool exercisesAreDifferent = exercise1 != exercise2;

        // Assert
        exercisesAreDifferent.Should().BeTrue();
    }
}
