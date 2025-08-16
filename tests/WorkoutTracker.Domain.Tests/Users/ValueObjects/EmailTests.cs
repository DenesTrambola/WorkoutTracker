namespace WorkoutTracker.Domain.Tests.Users.ValueObjects;

using FluentAssertions;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class EmailTests
{
    private readonly string _validEmailAddress = "example@gmail.com";

    [Fact]
    public void Create_Should_ReturnSuccess_When_AddressIsValid()
    {
        // Arrange
        string emailAddress = _validEmailAddress;

        // Act
        Result<Email> emailResult = Email.Create(emailAddress);

        // Assert
        emailResult.IsSuccess.Should().BeTrue();
        emailResult.ValueOrDefault().Should().NotBeNull();
        emailResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_AddressIsEmpty()
    {
        // Arrange
        string emailAddress = string.Empty;

        // Act
        Result<Email> emailResult = Email.Create(emailAddress);

        // Assert
        emailResult.IsFailure.Should().BeTrue();
        emailResult.ValueOrDefault().Should().BeNull();
        emailResult.Errors.Should().Contain(DomainErrors.Email.Empty);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_AddressIsTooLong()
    {
        // Arrange
        string emailAddress = new string('a', Email.MaxLength + 1);

        // Act
        Result<Email> emailResult = Email.Create(emailAddress);

        // Assert
        emailResult.IsFailure.Should().BeTrue();
        emailResult.ValueOrDefault().Should().BeNull();
        emailResult.Errors.Should().Contain(DomainErrors.Email.TooLong);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_AddressFormatIsInvalid()
    {
        // Arrange
        string emailAddress = new string('a', Email.MaxLength);

        // Act
        Result<Email> emailResult = Email.Create(emailAddress);

        // Assert
        emailResult.IsFailure.Should().BeTrue();
        emailResult.ValueOrDefault().Should().BeNull();
        emailResult.Errors.Should().Contain(DomainErrors.Email.InvalidFormat);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnSuccess_When_EmailIsNotNull()
    {
        // Arrange
        Result<Email> emailResult = Email.Create(_validEmailAddress);

        // Act
        emailResult = Email.EnsureNotNull(emailResult.ValueOrDefault());

        // Assert
        emailResult.IsSuccess.Should().BeTrue();
        emailResult.ValueOrDefault().Should().NotBeNull();
        emailResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnFailure_When_EmailIsNull()
    {
        // Arrange
        Email? email = null;

        // Act
        Result<Email> emailResult = Email.EnsureNotNull(email);

        // Assert
        emailResult.IsFailure.Should().BeTrue();
        emailResult.ValueOrDefault().Should().BeNull();
        emailResult.Errors.Should().Contain(DomainErrors.Email.Null);
    }

    [Fact]
    public void EmailsWithSameValues_Should_BeEqual()
    {
        // Arrange
        Email email1 = Email.Create(_validEmailAddress).ValueOrDefault();
        Email email2 = Email.Create(_validEmailAddress).ValueOrDefault();

        // Act
        bool emailsAreEqual = email1 == email2;

        // Assert
        emailsAreEqual.Should().BeTrue();
    }

    [Fact]
    public void EmailsWithDifferentValues_ShouldNot_BeEqual()
    {
        // Arrange
        Email email1 = Email.Create("example1@gmail.com").ValueOrDefault();
        Email email2 = Email.Create("example2@gmail.com").ValueOrDefault();

        // Act
        bool emailsAreDifferent = email1 != email2;

        // Assert
        emailsAreDifferent.Should().BeTrue();
    }
}
