namespace WorkoutTracker.Domain.Tests.Users.ValueObjects;

using FluentAssertions;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class UsernameTests
{
    private readonly string _validUsernameValue = "denestrambola";

    [Fact]
    public void Create_Should_ReturnSuccess_When_ValueIsValid()
    {
        // Act
        Result<Username> usernameResult = Username.Create(_validUsernameValue);

        // Assert
        usernameResult.IsSuccess.Should().BeTrue();
        usernameResult.ValueOrDefault().Should().NotBeNull();
        usernameResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_ValueIsEmpty()
    {
        // Arrest
        string usernameValue = string.Empty;

        // Act
        Result<Username> usernameResult = Username.Create(usernameValue);

        // Assert
        usernameResult.IsFailure.Should().BeTrue();
        usernameResult.ValueOrDefault().Should().BeNull();
        usernameResult.Errors.Should().Contain(DomainErrors.Username.Empty);
    }

    [Fact]
    public void CreateShould_ReturnFailure_When_ValueIsTooLong()
    {
        // Arrest
        string usernameValue = new string('a', Username.MaxLength + 1);

        // Act
        Result<Username> usernameResult = Username.Create(usernameValue);

        // Assert
        usernameResult.IsFailure.Should().BeTrue();
        usernameResult.ValueOrDefault().Should().BeNull();
        usernameResult.Errors.Should().Contain(DomainErrors.Username.TooLong);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnSuccess_When_UsernameIsNotNull()
    {
        // Arrest
        Result<Username> usernameResult = Username.Create(_validUsernameValue);

        // Act
        usernameResult = Username.EnsureNotNull(usernameResult.ValueOrDefault());

        // Assert
        usernameResult.IsSuccess.Should().BeTrue();
        usernameResult.ValueOrDefault().Should().NotBeNull();
        usernameResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnFailure_When_UsernameIsNull()
    {
        // Arrange
        Username? username = null;

        // Act
        Result<Username> usernameResult = Username.EnsureNotNull(username);

        // Assert
        usernameResult.IsFailure.Should().BeTrue();
        usernameResult.ValueOrDefault().Should().BeNull();
        usernameResult.Errors.Should().Contain(DomainErrors.Username.Null);
    }

    [Fact]
    public void UsernamesWithSameValues_Should_BeEqual()
    {
        // Arrange
        Username username1 = Username.Create(_validUsernameValue).ValueOrDefault();
        Username username2 = Username.Create(_validUsernameValue).ValueOrDefault();

        // Act
        bool usernamesAreEqual = username1 == username2;

        // Assert
        usernamesAreEqual.Should().BeTrue();
    }

    [Fact]
    public void UsernamesWithDifferentValues_ShouldNot_BeEqual()
    {
        // Arrange
        Username username1 = Username.Create("name1").ValueOrDefault();
        Username username2 = Username.Create("name2").ValueOrDefault();

        // Act
        bool usernamesAreDifferent = username1 != username2;

        // Assert
        usernamesAreDifferent.Should().BeTrue();
    }
}
