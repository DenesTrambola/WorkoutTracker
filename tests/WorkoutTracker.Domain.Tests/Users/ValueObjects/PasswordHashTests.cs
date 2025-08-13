namespace WorkoutTracker.Domain.Tests.Users.ValueObjects;

using FluentAssertions;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class PasswordHashTests
{
    private readonly string _validPasswordHash = "$2a$04$Kj9K.Wp2vfJ9Q0WPs7o8t.xv9IqvhRoYX2wjd3xeCFhnR6x9HE";

    [Fact]
    public void Create_Should_ReturnSuccess_When_ValueIsValid()
    {
        // Act
        Result<PasswordHash> passwordHashResult = PasswordHash.Create(_validPasswordHash);

        // Assert
        passwordHashResult.IsSuccess.Should().BeTrue();
        passwordHashResult.ValueOrDefault().Should().NotBeNull();
        passwordHashResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_ValueIsEmpty()
    {
        // Arrange
        string passwordHash = string.Empty;

        // Act
        Result<PasswordHash> passwordHashResult = PasswordHash.Create(passwordHash);

        // Assert
        passwordHashResult.IsFailure.Should().BeTrue();
        passwordHashResult.ValueOrDefault().Should().BeNull();
        passwordHashResult.Errors.Should().Contain(DomainErrors.PasswordHash.Empty);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnSuccess_When_PasswordHashIsNotNull()
    {
        // Arrange
        Result<PasswordHash> passwordHashResult = PasswordHash.Create(_validPasswordHash);

        // Act
        passwordHashResult = PasswordHash.EnsureNotNull(passwordHashResult.ValueOrDefault());

        // Assert
        passwordHashResult.IsSuccess.Should().BeTrue();
        passwordHashResult.ValueOrDefault().Should().NotBeNull();
        passwordHashResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnFailure_When_PasswordHashIsNull()
    {
        // Arrange
        PasswordHash? passwordHash = null;

        // Act
        Result<PasswordHash> passwordHashResult = PasswordHash.EnsureNotNull(passwordHash);

        // Assert
        passwordHashResult.IsFailure.Should().BeTrue();
        passwordHashResult.ValueOrDefault().Should().BeNull();
        passwordHashResult.Errors.Should().Contain(DomainErrors.PasswordHash.Null);
    }

    [Fact]
    public void PasswordHashesWithSameValues_Should_BeEqual()
    {
        // Arrange
        PasswordHash passwordHash1 = PasswordHash.Create(_validPasswordHash).ValueOrDefault();
        PasswordHash passwordHash2 = PasswordHash.Create(_validPasswordHash).ValueOrDefault();

        // Act
        bool passwordHashesAreEqual = passwordHash1 == passwordHash2;

        // Assert
        passwordHashesAreEqual.Should().BeTrue();
    }

    [Fact]
    public void PasswordHashesWithDifferentValues_ShouldNot_BeEqual()
    {
        // Arrange
        PasswordHash passwordHash1 = PasswordHash.Create("Password").ValueOrDefault();
        PasswordHash passwordHash2 = PasswordHash.Create("Password Hash").ValueOrDefault();

        // Act
        bool passwordHashesAreDifferent = passwordHash1 != passwordHash2;

        // Assert
        passwordHashesAreDifferent.Should().BeTrue();
    }
}
