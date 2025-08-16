namespace WorkoutTracker.Domain.Tests.Shared.ValueObjects;

using FluentAssertions;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;

public sealed class NameTests
{
    [Fact]
    public void Create_Should_ReturnSuccess_When_ValueIsValid()
    {
        // Arrange
        string nameValue = new string('a', Name.MaxLength);

        // Act
        Result<Name> nameResult = Name.Create(nameValue);

        // Assert
        nameResult.IsSuccess.Should().BeTrue();
        nameResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_ValueIsEmpty()
    {
        // Arrange
        string nameValue = string.Empty;

        // Act
        Result<Name> nameResult = Name.Create(nameValue);

        // Assert
        nameResult.IsFailure.Should().BeTrue();
        nameResult.Errors.Should().Contain(DomainErrors.Name.Empty);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_ValueIsTooLong()
    {
        // Arrange
        string nameValue = new string('a', Name.MaxLength + 1);

        // Act
        Result<Name> nameResult = Name.Create(nameValue);

        // Assert
        nameResult.IsFailure.Should().BeTrue();
        nameResult.Errors.Should().Contain(DomainErrors.Name.TooLong);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnSuccess_When_NameIsNotNull()
    {
        // Arrange
        Result<Name> nameResult = Name.Create(new string('a', Name.MaxLength));

        // Act
        nameResult = Name.EnsureNotNull(nameResult.ValueOrDefault());

        // Assert
        nameResult.IsSuccess.Should().BeTrue();
        nameResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnFailure_When_NameIsNull()
    {
        // Arrange
        Name? name = null;

        // Act
        Result<Name> nameResult = Name.EnsureNotNull(name);

        // Assert
        nameResult.IsFailure.Should().BeTrue();
        nameResult.Errors.Should().Contain(DomainErrors.Name.Null);
    }

    [Fact]
    public void NamesWithSameValues_Should_BeEqual()
    {
        // Arrange
        Name name1 = Name.Create(new string('a', Name.MaxLength)).ValueOrDefault();
        Name name2 = Name.Create(new string('a', Name.MaxLength)).ValueOrDefault();

        // Act
        bool namesAreEqual = name1 == name2;

        // Assert
        namesAreEqual.Should().BeTrue();
    }

    [Fact]
    public void NamesWithDifferentValues_ShouldNot_BeEqual()
    {
        // Arrange
        Name name1 = Name.Create("MyName").ValueOrDefault();
        Name name2 = Name.Create("NotMyName").ValueOrDefault();

        // Act
        bool namesAreDifferent = name1 != name2;

        // Assert
        namesAreDifferent.Should().BeTrue();
    }
}
