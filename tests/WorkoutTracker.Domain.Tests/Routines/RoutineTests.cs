namespace WorkoutTracker.Domain.Tests.Routines;

using FluentAssertions;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class RoutineTests
{
    private readonly Name _validName = Name.Create("Push").ValueOrDefault();
    private readonly Description _validDescription = Description.Create(null).ValueOrDefault();
    private readonly UserId _validUserId = UserId.New().ValueOrDefault();

    [Fact]
    public void Create_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Act
        Result<Routine> routineResult = Routine.Create(
            _validName,
            _validDescription,
            _validUserId);

        // Assert
        routineResult.IsSuccess.Should().BeTrue();
        routineResult.ValueOrDefault().Should().NotBeNull();
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_NameIsNull()
    {
        // Arrange
        Name? name = null;

        // Act
        Result<Routine> routineResult = Routine.Create(
            name!,
            _validDescription,
            _validUserId);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Name.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_DescriptionIsNull()
    {
        // Arrange
        Description? description = null;

        // Act
        Result<Routine> routineResult = Routine.Create(
            _validName,
            description!,
            _validUserId);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Description.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_UserIdIsNull()
    {
        // Arrange
        UserId? userId = null;

        // Act
        Result<Routine> routineResult = Routine.Create(
            _validName,
            _validDescription,
            userId!);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.UserId.Null);
    }

    [Fact]
    public void UpdateName_Should_ReturnSuccess_When_NameIsValid()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();
        Name newName = Name.Create("Pull").ValueOrDefault();

        // Act
        Result<Routine> routineResult = routine.UpdateName(newName);

        // Assert
        routineResult.IsSuccess.Should().BeTrue();
        routineResult.ValueOrDefault().Should().NotBeNull();
        routineResult.ValueOrDefault().Name.Should().Be(newName);
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateName_Should_ReturnFailure_When_NameIsNull()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId).ValueOrDefault();
        Name? newName = null;

        // Act
        Result<Routine> routineResult = routine.UpdateName(newName!);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Name.Null);
    }

    [Fact]
    public void UpdateDescription_Should_ReturnSuccess_When_DescriptionIsValid()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();
        Description newDescription = Description.Create("Description").ValueOrDefault();

        // Act
        Result<Routine> routineResult = routine.UpdateDescription(newDescription);

        // Assert
        routineResult.IsSuccess.Should().BeTrue();
        routineResult.ValueOrDefault().Should().NotBeNull();
        routineResult.ValueOrDefault().Description.Should().Be(newDescription);
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }


    [Fact]
    public void UpdateDescription_Should_ReturnFailure_When_DescriptionIsNull()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId).ValueOrDefault();
        Description? newDescription = null;

        // Act
        Result<Routine> routineResult = routine.UpdateDescription(newDescription!);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Description.Null);
    }

    [Fact]
    public void RoutinesWithSameValues_ShouldNot_BeEqual()
    {
        // Arrange
        Routine routine1 = Routine.Create(
            _validName,
            _validDescription,
            _validUserId).ValueOrDefault();
        Routine routine2 = Routine.Create(
            _validName,
            _validDescription,
            _validUserId).ValueOrDefault();

        // Act
        bool routinesAreDifferent = routine1 != routine2;

        // Assert
        routinesAreDifferent.Should().BeTrue();
    }
}
