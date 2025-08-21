namespace WorkoutTracker.Application.Tests.Users.Commands.Login;

using FluentAssertions;
using Moq;
using WorkoutTracker.Application.Users.Commands.Login;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Application.Users.Models;
using WorkoutTracker.Application.Users.Primitives;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IAccessTokenProvider> _accessTokenProviderMock = new();
    private readonly LoginCommandHandler _sut; // System Under Test

    private readonly string _username = "denestrambola";
    private readonly string _password = "Pa$$word123";

    private readonly PasswordHash _passwordHash = PasswordHash.Create("password").ValueOrDefault();
    private readonly Email _email = Email.Create("tramboladenes@gmail.com").ValueOrDefault();
    private readonly FullName _fullName = FullName.Create("Deinesh", "Trombola").ValueOrDefault();
    private readonly DateOnly _birthDate = DateOnly.FromDateTime(DateTime.UtcNow);

    private readonly AccessToken _accessToken = new("token", DateTime.UtcNow.AddHours(1));

    public LoginCommandHandlerTests()
    {
        _sut = new LoginCommandHandler(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _accessTokenProviderMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_When_UserIsLoggedIn()
    {
        // Arrange
        var command = new LoginCommand(_username, _password);

        _userRepositoryMock.Setup(ur => ur.GetByUsernameAsync(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username).Map(u => User.Create(
                u, _passwordHash, _email, _fullName, 0, 0, _birthDate)));
        _passwordHasherMock
            .Setup(ph => ph.VerifyAsync(It.IsAny<Password>(), It.IsAny<PasswordHash>(), default))
            .ReturnsAsync(Result.Success());
        _accessTokenProviderMock.Setup(atp => atp.GenerateToken(
            It.IsAny<UserId>(), It.IsAny<Email>(), It.IsAny<UserRole>()))
            .Returns(_accessToken);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.None);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_UsernameIsInvalid()
    {
        // Arrange
        var command = new LoginCommand(string.Empty, _password);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.Username.Empty);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_PasswordIsInvalid()
    {
        // Arrange
        var command = new LoginCommand(_username, string.Empty);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.Password.Empty);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_UserNotFound()
    {
        // Arrange
        var command = new LoginCommand(_username, _password);

        _userRepositoryMock.Setup(ur => ur.GetByUsernameAsync(It.IsAny<Username>(), default))
            .ReturnsAsync(Result.Failure<User>(ApplicationErrors.User.NotFound));

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(ApplicationErrors.User.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_PasswordVerificationFails()
    {
        // Arrange
        var command = new LoginCommand(_username, _password);

        _userRepositoryMock.Setup(ur => ur.GetByUsernameAsync(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username).Map(u => User.Create(
                u, _passwordHash, _email, _fullName, 0, 0, _birthDate)));
        _passwordHasherMock
            .Setup(ph => ph.VerifyAsync(It.IsAny<Password>(), It.IsAny<PasswordHash>(), default))
            .ReturnsAsync(Result.Failure());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(ApplicationErrors.Password.VerificationFailed);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_GeneratingAccessTokenFails()
    {
        // Arrange
        var command = new LoginCommand(_username, _password);

        _userRepositoryMock.Setup(ur => ur.GetByUsernameAsync(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username).Map(u => User.Create(
                u, _passwordHash, _email, _fullName, 0, 0, _birthDate)));
        _passwordHasherMock
            .Setup(ph => ph.VerifyAsync(It.IsAny<Password>(), It.IsAny<PasswordHash>(), default))
            .ReturnsAsync(Result.Success());
        _accessTokenProviderMock.Setup(atp => atp.GenerateToken(
            It.IsAny<UserId>(), It.IsAny<Email>(), It.IsAny<UserRole>()))
            .Returns(Result.Failure<AccessToken>(ApplicationErrors.AccessToken.GenerationFailed));

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(ApplicationErrors.AccessToken.GenerationFailed);
    }
}
