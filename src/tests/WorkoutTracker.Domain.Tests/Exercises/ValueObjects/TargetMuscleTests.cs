namespace WorkoutTracker.Domain.Tests.Exercises.ValueObjects;

using FluentAssertions;
using WorkoutTracker.Domain.Exercises.Errors;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Shared.Results;

public sealed class TargetMuscleTests
{
    private readonly string _validMuscle = "Chest";

    [Fact]
    public void Create_Should_ReturnSuccess_When_MuscleIsValid()
    {
        // Act
        Result<TargetMuscle> muscleResult = TargetMuscle.Create(_validMuscle);

        // Assert
        muscleResult.IsSuccess.Should().BeTrue();
        muscleResult.ValueOrDefault().Should().NotBeNull();
        muscleResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_MuscleIsEmpty()
    {
        // Arrange
        string muscle = string.Empty;

        // Act
        Result<TargetMuscle> muscleResult = TargetMuscle.Create(muscle);

        // Assert
        muscleResult.IsFailure.Should().BeTrue();
        muscleResult.ValueOrDefault().Should().BeNull();
        muscleResult.Errors.Should().Contain(DomainErrors.TargetMuscle.Empty);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_MuscleIsTooLong()
    {
        // Arrange
        string muscle = new string('a', TargetMuscle.MaxLength + 1);

        // Act
        Result<TargetMuscle> muscleResult = TargetMuscle.Create(muscle);

        // Assert
        muscleResult.IsFailure.Should().BeTrue();
        muscleResult.ValueOrDefault().Should().BeNull();
        muscleResult.Errors.Should().Contain(DomainErrors.TargetMuscle.TooLong);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnSuccess_When_MuscleIsNotNull()
    {
        // Arrange
        Result<TargetMuscle> muscleResult = TargetMuscle.Create(_validMuscle);

        // Act
        muscleResult = TargetMuscle.EnsureNotNull(muscleResult.ValueOrDefault());

        // Assert
        muscleResult.IsSuccess.Should().BeTrue();
        muscleResult.ValueOrDefault().Should().NotBeNull();
        muscleResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnFailure_When_MuscleIsNull()
    {
        // Arrange
        TargetMuscle? targetMuscle = null;

        // Act
        Result<TargetMuscle> muscleResult = TargetMuscle.EnsureNotNull(targetMuscle);

        // Assert
        muscleResult.IsFailure.Should().BeTrue();
        muscleResult.ValueOrDefault().Should().BeNull();
        muscleResult.Errors.Should().Contain(DomainErrors.TargetMuscle.Null);
    }

    [Fact]
    public void TargetMusclesWithSameValues_Should_BeEqual()
    {
        // Arrange
        TargetMuscle targetMuscle1 = TargetMuscle.Create(_validMuscle).ValueOrDefault();
        TargetMuscle targetMuscle2 = TargetMuscle.Create(_validMuscle).ValueOrDefault();

        // Act
        bool targetMusclesAreEqual = targetMuscle1 == targetMuscle2;

        // Assert
        targetMusclesAreEqual.Should().BeTrue();
    }

    [Fact]
    public void TargetMusclesWithDifferentValues_ShouldNot_BeEqual()
    {
        // Arrange
        TargetMuscle targetMuscle1 = TargetMuscle.Create("Biceps").ValueOrDefault();
        TargetMuscle targetMuscle2 = TargetMuscle.Create("Triceps").ValueOrDefault();

        // Act
        bool targetMusclesAreDifferent = targetMuscle1 != targetMuscle2;

        // Assert
        targetMusclesAreDifferent.Should().BeTrue();
    }
}
