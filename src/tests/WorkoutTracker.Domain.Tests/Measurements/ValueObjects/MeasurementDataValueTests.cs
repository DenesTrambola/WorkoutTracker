namespace WorkoutTracker.Domain.Tests.Measurements.ValueObjects;

using FluentAssertions;
using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Measurements.ValueObjects;
using WorkoutTracker.Domain.Shared.Results;

public sealed class MeasurementDataValueTests
{
    private readonly float _validValue = 1;

    [Fact]
    public void Create_Should_ReturnSuccess_When_ValueIsValid()
    {
        // Act
        Result<MeasurementDataValue> dataValueResult = MeasurementDataValue.Create(_validValue);

        // Assert
        dataValueResult.IsSuccess.Should().BeTrue();
        dataValueResult.ValueOrDefault().Should().NotBeNull();
        dataValueResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_ValueIsInvalid()
    {
        // Arrange
        float dataValue = 0;

        // Act
        Result<MeasurementDataValue> dataValueResult = MeasurementDataValue.Create(dataValue);

        // Assert
        dataValueResult.IsFailure.Should().BeTrue();
        dataValueResult.ValueOrDefault().Should().BeNull();
        dataValueResult.Errors.Should().Contain(DomainErrors.MeasurementDataValue.Invalid);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnSuccess_When_DataValueIsNotNull()
    {
        // Arrange
        Result<MeasurementDataValue> dataValueResult = MeasurementDataValue.Create(_validValue);

        // Act
        dataValueResult = MeasurementDataValue.EnsureNotNull(dataValueResult.ValueOrDefault());

        //Assert
        dataValueResult.IsSuccess.Should().BeTrue();
        dataValueResult.ValueOrDefault().Should().NotBeNull();
        dataValueResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnFailure_When_DataValueIsNull()
    {
        // Arrange
        MeasurementDataValue? dataValue = null;

        // Act
        Result<MeasurementDataValue> dataValueResult = MeasurementDataValue.EnsureNotNull(dataValue);

        // Assert
        dataValueResult.IsFailure.Should().BeTrue();
        dataValueResult.ValueOrDefault().Should().BeNull();
        dataValueResult.Errors.Should().Contain(DomainErrors.MeasurementDataValue.Null);
    }

    [Fact]
    public void DataValuesWithSameValues_Should_BeEqual()
    {
        // Arrange
        MeasurementDataValue dataValue1 = MeasurementDataValue.Create(_validValue).ValueOrDefault();
        MeasurementDataValue dataValue2 = MeasurementDataValue.Create(_validValue).ValueOrDefault();

        // Act
        bool dataValuesAreEqual = dataValue1 == dataValue2;

        // Assert
        dataValuesAreEqual.Should().BeTrue();
    }

    [Fact]
    public void DataValuesWithDifferentValues_ShouldNot_BeEqual()
    {
        // Arrange
        MeasurementDataValue dataValue1 = MeasurementDataValue.Create(1).ValueOrDefault();
        MeasurementDataValue dataValue2 = MeasurementDataValue.Create(2).ValueOrDefault();

        // Act
        bool dataValuesAreDifferent = dataValue1 != dataValue2;

        // Assert
        dataValuesAreDifferent.Should().BeTrue();
    }
}
