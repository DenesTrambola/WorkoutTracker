namespace WorkoutTracker.Application.Tests.Users.Commands.RegisterUser;

using FluentAssertions;
using Moq;
using WorkoutTracker.Application.Shared.Models;
using WorkoutTracker.Application.Shared.Primitives;
using WorkoutTracker.Application.Users.Commands.RegisterUser;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Application.Users.Primitives;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class RegisterUserCommandHandlerTests
{
    private readonly RegisterUserCommandHandler _sut; // System Under Test
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IEmailService> _emailServiceMock = new();

    private readonly string _username = "denestrambola";
    private readonly string _password = "Pa$$word123";
    private readonly string _email = "tramboladenes@gmail.com";
    private readonly string _firstName = "Deinesh";
    private readonly string _lastName = "Trombola";
    private readonly DateOnly _birthDate = DateOnly.FromDateTime(DateTime.UtcNow);

    public RegisterUserCommandHandlerTests()
    {
        _sut = new RegisterUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object,
            _emailServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_When_UserIsRegistered()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = _password,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Gender = 0,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
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
    public async Task Handle_Should_ReturnFailure_When_UsernameIsInvalid()
    {
        // Arrange
        string username = new string('a', Username.MaxLength + 1);
        var command = new RegisterUserCommand
        {
            Username = username,
            Password = _password,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Gender = 0,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
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
    public async Task Handle_Should_ReturnFailure_When_PasswordIsInvalid()
    {
        // Arrange
        string password = new string('a', Password.MinLength - 1);
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = password,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Gender = 0,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
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
    public async Task Handle_Should_ReturnFailure_When_EmailIsInvalid()
    {
        // Arrange
        string email = new string('a', Email.MaxLength + 1);
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = _password,
            Email = email,
            FirstName = _firstName,
            LastName = _lastName,
            Gender = 0,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
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
    public async Task Handle_Should_ReturnFailure_When_FirstNameIsInvalid()
    {
        // Arrange
        string firstName = new string('a', FullName.MaxLength + 1);
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = _password,
            Email = _email,
            FirstName = firstName,
            LastName = _lastName,
            Gender = 0,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
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
    public async Task Handle_Should_ReturnFailure_When_LastNameIsInvalid()
    {
        // Arrange
        string lastName = new string('a', FullName.MaxLength + 1);
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = _password,
            Email = _email,
            FirstName = _firstName,
            LastName = lastName,
            Gender = 0,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
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
    public async Task Handle_Should_ReturnFailure_When_GenderIsInvalid()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = _password,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Gender = 10,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
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
    public async Task Handle_Should_ReturnFailure_When_BirthDateIsInvalid()
    {
        // Arrange
        DateOnly birthDate = DateOnly.FromDateTime(DateTime.MaxValue);
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = _password,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Gender = 0,
            BirthDate = birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
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
    public async Task Handle_Should_ReturnFailure_When_PasswordHashingFails()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = _password,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Gender = 0,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(Result.Failure<PasswordHash>(Domain.Users.Errors.DomainErrors.PasswordHash.Null));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
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
    public async Task Handle_Should_ReturnFailure_When_UsernameIsAlreadyExists()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = _password,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Gender = 0,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Result.Failure<Username>());
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
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
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_EmailIsAlreadyExists()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = _password,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Gender = 0,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Result.Failure<Email>());
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
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_AddingUserToDatabaseFails()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = _password,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Gender = 0,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
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
    public async Task Handle_Should_ReturnFailure_When_EmailSendingFails()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Username = _username,
            Password = _password,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Gender = 0,
            BirthDate = _birthDate
        };

        _passwordHasherMock.Setup(ph => ph.HashAsync(It.IsAny<Password>(), default))
            .ReturnsAsync(PasswordHash.Create(command.Password));
        _userRepositoryMock.Setup(ur => ur.ValidateUsernameUniqueness(It.IsAny<Username>(), default))
            .ReturnsAsync(Username.Create(command.Username));
        _userRepositoryMock.Setup(ur => ur.ValidateEmailUniqueness(It.IsAny<Email>(), default))
            .ReturnsAsync(Email.Create(command.Email));
        _userRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<User>(), default))
            .ReturnsAsync(User.Create(
                Username.Create(command.Username).ValueOrDefault(),
                PasswordHash.Create(command.Password).ValueOrDefault(),
                Email.Create(command.Email).ValueOrDefault(),
                FullName.Create(command.FirstName, command.LastName).ValueOrDefault(),
                0, 0, command.BirthDate));
        _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>(), default))
             .ReturnsAsync(Result.Failure(ApplicationErrors.Email.CannotSend));

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(ApplicationErrors.Email.CannotSend);
    }
}
