namespace WorkoutTracker.Domain.Tests.Users.ValueObjects;

using FluentAssertions;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class FullNameTests
{
    private readonly string _validFirstName = "Deinesh";
    private readonly string _validLastName = "Trombola";

    [Fact]
    public void Create_Should_ReturnSuccess_When_NamesAreValid()
    {
        // Act
        Result<FullName> fullNameResult = FullName.Create(_validFirstName, _validLastName);

        // Assert
        fullNameResult.IsSuccess.Should().BeTrue();
        fullNameResult.ValueOrDefault().Should().NotBeNull();
        fullNameResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_FirstNameIsEmpty()
    {
        // Arrange
        string firstName = string.Empty;
        string lastName = _validLastName;

        // Act
        Result<FullName> fullNameResult = FullName.Create(firstName, lastName);

        // Assert
        fullNameResult.IsFailure.Should().BeTrue();
        fullNameResult.ValueOrDefault().Should().BeNull();
        fullNameResult.Errors.Should().Contain(DomainErrors.FirstName.Empty);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_FirstNameIsTooLong()
    {
        // Arrange
        string firstName = new string('a', FullName.MaxLength + 1);
        string lastName = _validLastName;

        // Act
        Result<FullName> fullNameResult = FullName.Create(firstName, lastName);

        // Assert
        fullNameResult.IsFailure.Should().BeTrue();
        fullNameResult.ValueOrDefault().Should().BeNull();
        fullNameResult.Errors.Should().Contain(DomainErrors.FirstName.TooLong);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_LastNameIsEmpty()
    {
        // Arrange
        string firstName = _validFirstName;
        string lastName = string.Empty;

        // Act
        Result<FullName> fullNameResult = FullName.Create(firstName, lastName);

        // Assert
        fullNameResult.IsFailure.Should().BeTrue();
        fullNameResult.ValueOrDefault().Should().BeNull();
        fullNameResult.Errors.Should().Contain(DomainErrors.LastName.Empty);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_LastNameIsTooLong()
    {
        // Arrange
        string firstName = _validFirstName;
        string lastName = new string('a', FullName.MaxLength + 1);

        // Act
        Result<FullName> fullNameResult = FullName.Create(firstName, lastName);

        // Assert
        fullNameResult.IsFailure.Should().BeTrue();
        fullNameResult.ValueOrDefault().Should().BeNull();
        fullNameResult.Errors.Should().Contain(DomainErrors.LastName.TooLong);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnSuccess_When_FullNameIsNotNull()
    {
        // Arrange
        Result<FullName> fullNameResult = FullName.Create(_validFirstName, _validLastName);

        // Act
        fullNameResult = FullName.EnsureNotNull(fullNameResult.ValueOrDefault());

        // Assert
        fullNameResult.IsSuccess.Should().BeTrue();
        fullNameResult.ValueOrDefault().Should().NotBeNull();
        fullNameResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnFailure_When_FullNameIsNull()
    {
        // Arrange
        FullName? fullName = null;

        // Act
        Result<FullName> fullNameResult = FullName.EnsureNotNull(fullName);

        // Assert
        fullNameResult.IsFailure.Should().BeTrue();
        fullNameResult.ValueOrDefault().Should().BeNull();
        fullNameResult.Errors.Should().Contain(DomainErrors.FullName.Null);
    }

    [Fact]
    public void FullNamesWithSameValues_Should_BeEqual()
    {
        // Arrange
        FullName fullName1 = FullName.Create(_validFirstName, _validLastName).ValueOrDefault();
        FullName fullName2 = FullName.Create(_validFirstName, _validLastName).ValueOrDefault();

        // Act
        bool fullNamesAreEqual = fullName1 == fullName2;

        // Assert
        fullNamesAreEqual.Should().BeTrue();
    }

    [Fact]
    public void FullNamesWithDifferentValues_ShouldNot_BeEqual()
    {
        // Arrange
        FullName fullName1 = FullName.Create("First", "Last").ValueOrDefault();
        FullName fullName2 = FullName.Create("FirstName", "LastName").ValueOrDefault();

        // Act
        bool fullNamesAreDifferent = fullName1 != fullName2;

        // Assert
        fullNamesAreDifferent.Should().BeTrue();
    }
}
