namespace WorkoutTracker.Domain.Tests.Measurements;

using FluentAssertions;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Measurements.ValueObjects;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class MeasurementDataTests
{
    private readonly Measurement _measurement = Measurement.Create(
        Name.Create("Right Arm").ValueOrDefault(),
        Description.Create(null).ValueOrDefault(),
        MeasurementUnit.Centimeter,
        UserId.New().ValueOrDefault())
        .ValueOrDefault();

    [Fact]
    public void UpdateValue_Should_ReturnSuccess_When_ValueIsNotNull()
    {
        // Arrange
        MeasurementData data = _measurement.AddData(
            MeasurementDataValue.Create(1).ValueOrDefault(),
            DateTime.UtcNow,
            Comment.Create(null).ValueOrDefault())
            .ValueOrDefault();
        MeasurementDataValue newValue = MeasurementDataValue.Create(2).ValueOrDefault();

        // Act
        Result<MeasurementData> dataResult = data.UpdateValue(newValue);

        // Assert
        dataResult.IsSuccess.Should().BeTrue();
        dataResult.ValueOrDefault().Should().NotBeNull();
        dataResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        data.Value.Should().Be(newValue);
    }

    [Fact]
    public void UpdateValue_Should_ReturnFailure_When_ValueIsNull()
    {
        // Arrange
        MeasurementData data = _measurement.AddData(
            MeasurementDataValue.Create(1).ValueOrDefault(),
            DateTime.UtcNow,
            Comment.Create(null).ValueOrDefault())
            .ValueOrDefault();
        MeasurementDataValue? newValue = null;

        // Act
        Result<MeasurementData> dataResult = data.UpdateValue(newValue!);

        // Assert
        dataResult.IsFailure.Should().BeTrue();
        dataResult.ValueOrDefault().Should().BeNull();
        dataResult.Errors.Should().Contain(DomainErrors.MeasurementDataValue.Null);
        data.Value.Should().NotBe(newValue);
    }

    [Fact]
    public void UpdateMeasuredOn_Should_ReturnSuccess_When_MeasuredOnIsValid()
    {
        // Arrange
        MeasurementData data = _measurement.AddData(
            MeasurementDataValue.Create(1).ValueOrDefault(),
            DateTime.UtcNow,
            Comment.Create(null).ValueOrDefault())
            .ValueOrDefault();
        DateTime newMeasuredOn = DateTime.MinValue;

        // Act
        Result<MeasurementData> dataResult = data.UpdateMeasuredOn(newMeasuredOn);

        // Assert
        dataResult.IsSuccess.Should().BeTrue();
        dataResult.ValueOrDefault().Should().NotBeNull();
        dataResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        data.MeasuredOn.Should().Be(newMeasuredOn);
    }

    [Fact]
    public void UpdateMeasuredOn_Should_ReturnFailure_When_MeasuredOnIsInvalid()
    {
        // Arrange
        MeasurementData data = _measurement.AddData(
            MeasurementDataValue.Create(1).ValueOrDefault(),
            DateTime.UtcNow,
            Comment.Create(null).ValueOrDefault())
            .ValueOrDefault();
        DateTime newMeasuredOn = DateTime.MaxValue;

        // Act
        Result<MeasurementData> dataResult = data.UpdateMeasuredOn(newMeasuredOn);

        // Assert
        dataResult.IsFailure.Should().BeTrue();
        dataResult.ValueOrDefault().Should().BeNull();
        dataResult.Errors.Should().Contain(DomainErrors.MeasurementData.InvalidDate);
        data.MeasuredOn.Should().NotBe(newMeasuredOn);
    }

    [Fact]
    public void UpdateComment_Should_ReturnSuccess_When_CommentIsNotNull()
    {
        // Arrange
        MeasurementData data = _measurement.AddData(
            MeasurementDataValue.Create(1).ValueOrDefault(),
            DateTime.UtcNow,
            Comment.Create(null).ValueOrDefault())
            .ValueOrDefault();
        Comment newComment = Comment.Create("This is a comment.").ValueOrDefault();

        // Act
        Result<MeasurementData> dataResult = data.UpdateComment(newComment!);

        // Assert
        dataResult.IsSuccess.Should().BeTrue();
        dataResult.ValueOrDefault().Should().NotBeNull();
        dataResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        data.Comment.Should().Be(newComment);
    }

    [Fact]
    public void UpdateComment_Should_ReturnFailure_When_CommentIsNull()
    {
        // Arrange
        MeasurementData data = _measurement.AddData(
            MeasurementDataValue.Create(1).ValueOrDefault(),
            DateTime.UtcNow,
            Comment.Create(null).ValueOrDefault())
            .ValueOrDefault();
        Comment? newComment = null;

        // Act
        Result<MeasurementData> dataResult = data.UpdateComment(newComment!);

        // Assert
        dataResult.IsFailure.Should().BeTrue();
        dataResult.ValueOrDefault().Should().BeNull();
        dataResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Comment.Null);
        data.Comment.Should().NotBe(newComment);
    }

    [Fact]
    public void MeasurementDataWithSameValues_Should_NotBeEqual()
    {
        // Arrange
        MeasurementData data1 = _measurement.AddData(
            MeasurementDataValue.Create(1).ValueOrDefault(),
            DateTime.UtcNow,
            Comment.Create(null).ValueOrDefault())
            .ValueOrDefault();
        MeasurementData data2 = _measurement.AddData(
            MeasurementDataValue.Create(1).ValueOrDefault(),
            DateTime.UtcNow,
            Comment.Create(null).ValueOrDefault())
            .ValueOrDefault();

        // Act
        bool dataAreDifferent = data1 != data2;

        // Assert
        dataAreDifferent.Should().BeTrue();
    }
}
