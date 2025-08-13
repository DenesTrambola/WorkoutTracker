namespace WorkoutTracker.Domain.Tests.Routines.ValueObjects;

using FluentAssertions;
using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Routines.ValueObjects;
using WorkoutTracker.Domain.Shared.Results;

public sealed class ExercisePositionTests
{
    private readonly byte _validPosition = 1;

    [Fact]
    public void Create_Should_ReturnSuccess_When_ValueIsValid()
    {
        // Act
        Result<ExercisePosition> positionResult = ExercisePosition.Create(_validPosition);

        // Assert
        positionResult.IsSuccess.Should().BeTrue();
        positionResult.ValueOrDefault().Should().NotBeNull();
        positionResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_ValueIsInvalid()
    {
        // Arrange
        byte position = 0;

        // Act
        Result<ExercisePosition> positionResult = ExercisePosition.Create(position);

        // Assert
        positionResult.IsFailure.Should().BeTrue();
        positionResult.ValueOrDefault().Should().BeNull();
        positionResult.Errors.Should().Contain(DomainErrors.ExercisePosition.Invalid);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnSuccess_When_PositionIsNotNull()
    {
        // Arrange
        ExercisePosition position = ExercisePosition.Create(_validPosition).ValueOrDefault();

        // Act
        Result<ExercisePosition> positionResult = ExercisePosition.EnsureNotNull(position);

        // Assert
        positionResult.IsSuccess.Should().BeTrue();
        positionResult.ValueOrDefault().Should().NotBeNull();
        positionResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnFailure_When_PositionNull()
    {
        // Arrange
        ExercisePosition? position = null;

        // Act
        Result<ExercisePosition> positionResult = ExercisePosition.EnsureNotNull(position);

        // Assert
        positionResult.IsFailure.Should().BeTrue();
        positionResult.ValueOrDefault().Should().BeNull();
        positionResult.Errors.Should().Contain(DomainErrors.ExercisePosition.Null);
    }

    [Fact]
    public void PositionsWithSameValues_Should_BeEqual()
    {
        // Arrange
        ExercisePosition position1 = ExercisePosition.Create(_validPosition).ValueOrDefault();
        ExercisePosition position2 = ExercisePosition.Create(_validPosition).ValueOrDefault();

        // Act
        bool positionsAreEqual = position1 == position2;

        // Assert
        positionsAreEqual.Should().BeTrue();
    }

    [Fact]
    public void PositionsWithDifferentValues_ShouldNot_BeEqual()
    {
        // Arrange
        ExercisePosition position1 = ExercisePosition.Create(1).ValueOrDefault();
        ExercisePosition position2 = ExercisePosition.Create(2).ValueOrDefault();

        // Act
        bool positionsAreDifferent = position1 != position2;

        // Assert
        positionsAreDifferent.Should().BeTrue();
    }
}
