namespace WorkoutTracker.Domain.Tests.Measurements;

using FluentAssertions;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.Errors;
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
