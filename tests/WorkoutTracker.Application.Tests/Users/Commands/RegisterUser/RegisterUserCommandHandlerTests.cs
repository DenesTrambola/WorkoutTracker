namespace WorkoutTracker.Application.Tests.Users.Commands.RegisterUser;

using FluentAssertions;
using Moq;
using WorkoutTracker.Application.Shared.Models;
using WorkoutTracker.Application.Users.Commands.RegisterUser;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Application.Users.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class RegisterUserCommandHandlerTests
{
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly RegisterUserCommandHandler _sut; // System Under Test

    private readonly string _validUsername = "denestrambola";
    private readonly string _validPassword = "Pa$$word123";
    private readonly string _validEmail = "tramboladenes@gmail.com";
    private readonly string _validFirstName = "Deinesh";
    private readonly string _validLastName = "Trombola";
    private readonly DateOnly _validBirthDate = DateOnly.FromDateTime(DateTime.UtcNow);

    public RegisterUserCommandHandlerTests()
    {
        _sut = new RegisterUserCommandHandler(
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _emailServiceMock.Object);
    }

    [Fact]
    public async void Handle_Should_ReturnSuccess_When_UserIsRegistered()
    {
        // Arrange
        RegisterUserCommand command = new(
            _validUsername,
            _validPassword,
            _validEmail,
            _validFirstName, _validLastName,
            0, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_UsernameIsInvalid()
    {
        // Arrange
        string username = new string('a', Username.MaxLength + 1);
        RegisterUserCommand command = new(
            username,
            _validPassword,
            _validEmail,
            _validFirstName, _validLastName,
            0, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.Username.TooLong);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_PasswordIsInvalid()
    {
        // Arrange
        string password = new string('a', Password.MinLength - 1);
        RegisterUserCommand command = new(
            _validUsername,
            password,
            _validEmail,
            _validFirstName, _validLastName,
            0, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.Password.TooShort);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_EmailIsInvalid()
    {
        // Arrange
        string email = new string('a', Email.MaxLength + 1);
        RegisterUserCommand command = new(
            _validUsername,
            _validPassword,
            email,
            _validFirstName, _validLastName,
            0, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.Email.InvalidFormat);
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.Email.TooLong);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_FirstNameIsInvalid()
    {
        // Arrange
        string firstName = new string('a', FullName.MaxLength + 1);
        RegisterUserCommand command = new(
            _validUsername,
            _validPassword,
            _validEmail,
            firstName, _validLastName,
            0, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.FirstName.TooLong);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_LastNameIsInvalid()
    {
        // Arrange
        string lastName = new string('a', FullName.MaxLength + 1);
        RegisterUserCommand command = new(
            _validUsername,
            _validPassword,
            _validEmail,
            _validFirstName, lastName,
            0, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.LastName.TooLong);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_GenderIsInvalid()
    {
        // Arrange
        RegisterUserCommand command = new(
            _validUsername,
            _validPassword,
            _validEmail,
            _validFirstName, _validLastName,
            10, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.Gender.Invalid);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_BirthDateIsInvalid()
    {
        // Arrange
        DateOnly birthDate = DateOnly.FromDateTime(DateTime.MaxValue);
        RegisterUserCommand command = new(
            _validUsername,
            _validPassword,
            _validEmail,
            _validFirstName, _validLastName,
            0, birthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.BirthDate.Invalid);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_PasswordHashingFails()
    {
        // Arrange
        RegisterUserCommand command = new(
            _validUsername,
            _validPassword,
            _validEmail,
            _validFirstName, _validLastName,
            0, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(Result.Failure<PasswordHash>(Domain.Users.Errors.DomainErrors.PasswordHash.Null));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.PasswordHash.Null);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_UsernameIsAlreadyExists()
    {
        // Arrange
        RegisterUserCommand command = new(
            _validUsername,
            _validPassword,
            _validEmail,
            _validFirstName, _validLastName,
            0, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(false);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(ApplicationErrors.Username.AlreadyExists);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_EmailIsAlreadyExists()
    {
        // Arrange
        RegisterUserCommand command = new(
            _validUsername,
            _validPassword,
            _validEmail,
            _validFirstName, _validLastName,
            0, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(false);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(ApplicationErrors.Email.AlreadyExists);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_AddingUserToDatabaseFails()
    {
        // Arrange
        RegisterUserCommand command = new(
            _validUsername,
            _validPassword,
            _validEmail,
            _validFirstName, _validLastName,
            0, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(Result.Failure<User>(Domain.Users.Errors.DomainErrors.User.Null));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Success());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.User.Null);
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_When_EmailSendingFails()
    {
        // Arrange
        RegisterUserCommand command = new(
            _validUsername,
            _validPassword,
            _validEmail,
            _validFirstName, _validLastName,
            0, _validBirthDate);

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.IsUsernameUnique(It.IsAny<Username>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.IsEmailUnique(It.IsAny<Email>(), default))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Failure(ApplicationErrors.Email.SendingFailed));

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(ApplicationErrors.Email.SendingFailed);
    }
}
