namespace WorkoutTracker.Infrastructure.Tests;

using FluentAssertions;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Domain.Users.ValueObjects;

public class PasswordHasherServiceTests
{
    private readonly PasswordHasherService _sut = new();

    [Fact]
    public async Task HashAsync_ShouldReturnSuccessResult_WhenPasswordIsValid()
    {
        // Arrange
        var password = Password.Create("StrongPassword123!").ValueOrDefault();

        // Act
        var result = await _sut.HashAsync(password);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault().Should().NotBeNull();
        result.ValueOrDefault().Value.Should().NotBeNullOrEmpty();
        result.ValueOrDefault().Value.Should().NotBe("StrongPassword123!");
    }

    [Fact]
    public async Task VerifyAsync_ShouldReturnSuccess_WhenPasswordMatchesHash()
    {
        // Arrange
        var password = Password.Create("MySecret123!").ValueOrDefault();
        var hashResult = await _sut.HashAsync(password);

        // Act
        var verifyResult = await _sut.VerifyAsync(password, hashResult.ValueOrDefault());

        // Assert
        verifyResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task VerifyAsync_ShouldReturnFailure_WhenPasswordDoesNotMatch()
    {
        // Arrange
        var password = Password.Create("Correct123!").ValueOrDefault();
        var wrongPassword = Password.Create("Wrong123!").ValueOrDefault();
        var hashResult = await _sut.HashAsync(password);

        // Act
        var verifyResult = await _sut.VerifyAsync(wrongPassword, hashResult.ValueOrDefault());

        // Assert
        verifyResult.IsSuccess.Should().BeFalse();
        verifyResult.Errors.Should().Contain(ApplicationErrors.Password.VerificationFailed);
    }

    [Fact]
    public async Task HashAsync_ShouldReturnFailure_WhenPasswordIsNull()
    {
        // Arrange
        Password? password = null;

        // Act
        var result = await _sut.HashAsync(password!);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(ApplicationErrors.PasswordHash.CannotHash);
    }
}
