namespace WorkoutTracker.Domain.Tests.Users;

using System.Data;
using FluentAssertions;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class UserTests
{
    private readonly Username _validUsername = Username.Create("denestrambola").ValueOrDefault();
    private readonly PasswordHash _validPasswordHash = PasswordHash.Create("123").ValueOrDefault();
    private readonly Email _validEmail = Email.Create("example@gmail.com").ValueOrDefault();
    private readonly FullName _validFullName = FullName.Create("Deinesh", "Trombola").ValueOrDefault();
    private readonly Gender _validGender = Gender.Male;
    private readonly UserRole _validRole = UserRole.User;
    private readonly DateOnly _validBirthDate = new DateOnly(2000, 1, 1);

    [Fact]
    public void Create_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Act
        Result<User> userResult = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_UsernameIsNull()
    {
        // Arrange
        Username? username = null;

        // Act
        Result<User> userResult = User.Create(
            username!,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Username.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_PasswordHashIsNull()
    {
        // Arrange
        PasswordHash? passwordHash = null;

        // Act
        Result<User> userResult = User.Create(
            _validUsername,
            passwordHash!,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.PasswordHash.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_EmailIsNull()
    {
        // Arrange
        Email? email = null;

        // Act
        Result<User> userResult = User.Create(
            _validUsername,
            _validPasswordHash,
            email!,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Email.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_FullNameIsNull()
    {
        // Arrange
        FullName? fullName = null;

        // Act
        Result<User> userResult = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            fullName!,
            _validGender,
            _validRole,
            _validBirthDate);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.FullName.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_GenderIsInvalid()
    {
        // Arrange
        Gender gender = (Gender)10;

        // Act
        Result<User> userResult = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            gender,
            _validRole,
            _validBirthDate);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Gender.Invalid);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_RoleIsInvalid()
    {
        // Arrange
        UserRole role = (UserRole)10;

        // Act
        Result<User> userResult = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            role,
            _validBirthDate);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.UserRole.Invalid);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_BirthDateIsInvalid()
    {
        // Arrange
        DateOnly birthDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);

        // Act
        Result<User> userResult = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            birthDate);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.BirthDate.Invalid);
    }

    [Fact]
    public void UpdateUsername_Should_ReturnSuccess_When_UsernameIsValid()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        Username newUsername = Username.Create("deinesh").ValueOrDefault();

        // Act
        Result<User> userResult = user.UpdateUsername(newUsername);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().Username.Should().Be(newUsername);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateUsername_Should_ReturnFailure_When_UsernameIsNull()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        Username? newUsername = null;

        // Act
        Result<User> userResult = user.UpdateUsername(newUsername!);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Username.Null);
    }

    [Fact]
    public void UpdatePasswordHash_Should_ReturnSuccess_When_PasswordIsValid()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        PasswordHash newPasswordHash = PasswordHash.Create("password").ValueOrDefault();

        // Act
        Result<User> userResult = user.UpdatePasswordHash(newPasswordHash);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().PasswordHash.Should().Be(newPasswordHash);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdatePasswordHash_Should_ReturnFailure_When_PasswordIsNull()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        PasswordHash? newPasswordHash = null;

        // Act
        Result<User> userResult = user.UpdatePasswordHash(newPasswordHash!);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.PasswordHash.Null);
    }

    [Fact]
    public void UpdateEmail_Should_ReturnSuccess_When_EmailIsValid()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        Email newEmail = Email.Create("deinesh@gmail.com").ValueOrDefault();

        // Act
        Result<User> userResult = user.UpdateEmail(newEmail);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().Email.Should().Be(newEmail);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateEmail_Should_ReturnFailure_When_EmailIsNull()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        Email? newEmail = null;

        // Act
        Result<User> userResult = user.UpdateEmail(newEmail!);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Email.Null);
    }

    [Fact]
    public void UpdateFullName_Should_ReturnSuccess_When_FullNameIsValid()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        FullName newFullName = FullName.Create("First", "Last").ValueOrDefault();

        // Act
        Result<User> userResult = user.UpdateFullName(newFullName);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().FullName.Should().Be(newFullName);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateFullName_Should_ReturnFailure_When_FullNameIsNull()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        FullName? newFullName = null;

        // Act
        Result<User> userResult = user.UpdateFullName(newFullName!);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.FullName.Null);
    }

    [Fact]
    public void UpdateGender_Should_ReturnSuccess_When_GenderIsValid()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        Gender newGender = Gender.Female;

        // Act
        Result<User> userResult = user.UpdateGender(newGender);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().Gender.Should().Be(newGender);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateGender_Should_ReturnFailure_When_GenderIsInvalid()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        Gender newGender = (Gender)10;

        // Act
        Result<User> userResult = user.UpdateGender(newGender);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.Gender.Invalid);
    }

    [Fact]
    public void UpdateRole_Should_ReturnSuccess_When_RoleIsValid()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        UserRole newRole = UserRole.Admin;

        // Act
        Result<User> userResult = user.UpdateRole(newRole);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().Role.Should().Be(newRole);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateRole_Should_ReturnFailure_When_RoleIsInvalid()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        UserRole newRole = (UserRole)10;

        // Act
        Result<User> userResult = user.UpdateRole(newRole);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.UserRole.Invalid);
    }

    [Fact]
    public void UpdateBirthDate_Should_ReturnSuccess_When_BirthDateIsValid()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        DateOnly newBirthDate = new DateOnly(2025, 1, 1);

        // Act
        Result<User> userResult = user.UpdateBirthDate(newBirthDate);

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.ValueOrDefault().Should().NotBeNull();
        userResult.ValueOrDefault().BirthDate.Should().Be(newBirthDate);
        userResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateBirthDate_Should_ReturnFailure_When_BirthDateIsInvalid()
    {
        // Arrange
        User user = User.Create(
            _validUsername,
            _validPasswordHash,
            _validEmail,
            _validFullName,
            _validGender,
            _validRole,
            _validBirthDate)
            .ValueOrDefault();
        DateOnly newBirthDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);

        // Act
        Result<User> userResult = user.UpdateBirthDate(newBirthDate);

        // Assert
        userResult.IsFailure.Should().BeTrue();
        userResult.ValueOrDefault().Should().BeNull();
        userResult.Errors.Should().Contain(DomainErrors.BirthDate.Invalid);
    }
}
