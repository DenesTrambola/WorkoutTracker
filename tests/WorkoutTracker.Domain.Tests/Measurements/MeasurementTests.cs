namespace WorkoutTracker.Domain.Tests.Measurements;

using FluentAssertions;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Measurements.ValueObjects;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public class MeasurementTests
{
    private readonly Name _validName = Name.Create("Right Arm").ValueOrDefault();
    private readonly Description _validDescription = Description.Create(null).ValueOrDefault();
    private readonly MeasurementUnit _validUnit = MeasurementUnit.Centimeter;
    private readonly UserId _validUserId = UserId.New().ValueOrDefault();

    [Fact]
    public void Create_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Act
        Result<Measurement> measurementResult = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId);

        // Assert
        measurementResult.IsSuccess.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().NotBeNull();
        measurementResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_NameIsNull()
    {
        // Arrange
        Name? name = null;

        // Act
        Result<Measurement> measurementResult = Measurement.Create(
            name!,
            _validDescription,
            _validUnit,
            _validUserId);

        // Assert
        measurementResult.IsFailure.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().BeNull();
        measurementResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Name.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_DescriptionIsNull()
    {
        // Arrange
        Description? description = null;

        // Act
        Result<Measurement> measurementResult = Measurement.Create(
            _validName,
            description!,
            _validUnit,
            _validUserId);

        // Assert
        measurementResult.IsFailure.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().BeNull();
        measurementResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Description.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_UnitIsInvalid()
    {
        // Arrange
        MeasurementUnit unit = (MeasurementUnit)10;

        // Act
        Result<Measurement> measurementResult = Measurement.Create(
            _validName,
            _validDescription!,
            unit,
            _validUserId);

        // Assert
        measurementResult.IsFailure.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().BeNull();
        measurementResult.Errors.Should().Contain(DomainErrors.MeasurementUnit.Invalid);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_UserIdIsNull()
    {
        // Arrange
        UserId? userId = null;

        // Act
        Result<Measurement> measurementResult = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            userId!);

        // Assert
        measurementResult.IsFailure.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().BeNull();
        measurementResult.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.UserId.Null);
    }

    [Fact]
    public void UpdateName_Should_ReturnSuccess_When_NameIsValid()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        Name newName = Name.Create("Right Quads").ValueOrDefault();

        // Act
        Result<Measurement> measurementResult = measurement.UpdateName(newName);

        // Assert
        measurementResult.IsSuccess.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().NotBeNull();
        measurementResult.ValueOrDefault().Name.Should().Be(newName);
        measurementResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateName_Should_ReturnFailure_When_NameIsNull()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        Name? newName = null;

        // Act
        Result<Measurement> measurementResult = measurement.UpdateName(newName!);

        // Assert
        measurementResult.IsFailure.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().BeNull();
        measurementResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Name.Null);
    }

    [Fact]
    public void UpdateDescription_Should_ReturnSuccess_When_DescriptionIsValid()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        Description newDescription = Description.Create("Description").ValueOrDefault();

        // Act
        Result<Measurement> measurementResult = measurement.UpdateDescription(newDescription);

        // Assert
        measurementResult.IsSuccess.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().NotBeNull();
        measurementResult.ValueOrDefault().Description.Should().Be(newDescription);
        measurementResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateDescription_Should_ReturnFailure_When_DescriptionIsNull()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        Description? newDescription = null;

        // Act
        Result<Measurement> measurementResult = measurement.UpdateDescription(newDescription!);

        // Assert
        measurementResult.IsFailure.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().BeNull();
        measurementResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Description.Null);
    }

    [Fact]
    public void UpdateUnit_Should_ReturnSuccess_When_UnitIsValid()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        MeasurementUnit newUnit = MeasurementUnit.Percentage;

        // Act
        Result<Measurement> measurementResult = measurement.UpdateUnit(newUnit);

        // Assert
        measurementResult.IsSuccess.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().NotBeNull();
        measurementResult.ValueOrDefault().Unit.Should().Be(newUnit);
        measurementResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateUnit_Should_ReturnFailure_When_UnitIsInvalid()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        MeasurementUnit newUnit = (MeasurementUnit)10;

        // Act
        Result<Measurement> measurementResult = measurement.UpdateUnit(newUnit);

        // Assert
        measurementResult.IsFailure.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().BeNull();
        measurementResult.Errors.Should().Contain(DomainErrors.MeasurementUnit.Invalid);
    }

    [Fact]
    public void AddData_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        MeasurementDataValue dataValue = MeasurementDataValue.Create(1).ValueOrDefault();
        DateTime measuredOn = DateTime.UtcNow;
        Comment comment = Comment.Create(null).ValueOrDefault();

        // Act
        Result<MeasurementData> dataResult = measurement.AddData(dataValue, measuredOn, comment);

        // Assert
        dataResult.IsSuccess.Should().BeTrue();
        dataResult.ValueOrDefault().Should().NotBeNull();
        dataResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        measurement.Data.Should().Contain(dataResult.ValueOrDefault());
    }

    [Fact]
    public void AddData_Should_ReturnFailure_When_DataValueIsNull()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        MeasurementDataValue? dataValue = null;
        DateTime measuredOn = DateTime.UtcNow;
        Comment comment = Comment.Create(null).ValueOrDefault();

        // Act
        Result<MeasurementData> dataResult = measurement.AddData(dataValue!, measuredOn, comment);

        // Assert
        dataResult.IsFailure.Should().BeTrue();
        dataResult.ValueOrDefault().Should().BeNull();
        dataResult.Errors.Should().Contain(DomainErrors.MeasurementDataValue.Null);
        measurement.Data.Should().NotContain(dataResult.ValueOrDefault());
    }

    [Fact]
    public void AddData_Should_ReturnFailure_When_MeasuredOnIsInvalid()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        MeasurementDataValue dataValue = MeasurementDataValue.Create(1).ValueOrDefault();
        DateTime measuredOn = DateTime.UtcNow.AddDays(1);
        Comment comment = Comment.Create(null).ValueOrDefault();

        // Act
        Result<MeasurementData> dataResult = measurement.AddData(dataValue, measuredOn, comment);

        // Assert
        dataResult.IsFailure.Should().BeTrue();
        dataResult.ValueOrDefault().Should().BeNull();
        dataResult.Errors.Should().Contain(DomainErrors.MeasurementData.InvalidDate);
        measurement.Data.Should().NotContain(dataResult.ValueOrDefault());
    }

    [Fact]
    public void AddData_Should_ReturnFailure_When_CommentIsNull()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        MeasurementDataValue dataValue = MeasurementDataValue.Create(1).ValueOrDefault();
        DateTime measuredOn = DateTime.UtcNow;
        Comment? comment = null;

        // Act
        Result<MeasurementData> dataResult = measurement.AddData(dataValue, measuredOn, comment!);

        // Assert
        dataResult.IsFailure.Should().BeTrue();
        dataResult.ValueOrDefault().Should().BeNull();
        dataResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Comment.Null);
        measurement.Data.Should().NotContain(dataResult.ValueOrDefault());
    }

    [Fact]
    public void RemoveData_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
        .ValueOrDefault();
        MeasurementData data = measurement.AddData(
            MeasurementDataValue.Create(1).ValueOrDefault(),
            DateTime.UtcNow,
            Comment.Create(null).ValueOrDefault())
            .ValueOrDefault();

        // Act
        Result<Measurement> measurementResult = measurement.RemoveData(data.Id);

        // Assert
        measurementResult.IsSuccess.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().NotBeNull();
        measurementResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        measurement.Data.Should().NotContain(data);
    }

    [Fact]
    public void RemoveData_Should_ReturnFailure_When_DataIdIsNull()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        MeasurementDataId? dataId = null;

        // Act
        Result<Measurement> measurementResult = measurement.RemoveData(dataId!);

        // Assert
        measurementResult.IsFailure.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().BeNull();
        measurementResult.Errors.Should().Contain(DomainErrors.MeasurementDataId.Null);
    }

    [Fact]
    public void RemoveData_Should_ReturnFailure_When_DataIsNotFound()
    {
        // Arrange
        Measurement measurement = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        MeasurementData data = measurement.AddData(
            MeasurementDataValue.Create(1).ValueOrDefault(),
            DateTime.UtcNow,
            Comment.Create(null).ValueOrDefault())
            .ValueOrDefault();
        Result<Measurement> measurementResult = measurement.RemoveData(data.Id);

        // Act
        measurementResult = measurement.RemoveData(data.Id);

        // Assert
        measurementResult.IsFailure.Should().BeTrue();
        measurementResult.ValueOrDefault().Should().BeNull();
        measurementResult.Errors.Should().Contain(DomainErrors.MeasurementData.NotFound);
        measurement.Data.Should().NotContain(data);
    }

    [Fact]
    public void MeasurementsWithSameValue_ShouldNot_BeEqual()
    {
        // Arrange
        Measurement measurement1 = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();
        Measurement measurement2 = Measurement.Create(
            _validName,
            _validDescription,
            _validUnit,
            _validUserId)
            .ValueOrDefault();

        // Act
        bool measurementsAreDifferent = measurement1 != measurement2;

        measurementsAreDifferent.Should().BeTrue();
    }
}
