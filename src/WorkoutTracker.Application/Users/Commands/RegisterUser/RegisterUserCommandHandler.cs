namespace WorkoutTracker.Application.Users.Commands.RegisterUser;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WorkoutTracker.Application.Shared.Models;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Application.Users.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class RegisterUserCommandHandler(
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        IEmailService emailService)
    : ICommandHandler<RegisterUserCommand>
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailService _emailService = emailService;

    public async Task<Result> Handle(
        [NotNull] RegisterUserCommand request,
        CancellationToken cancellationToken = default)
    {
        var usernameResult = await ValidateAndCreateUsernameAsync(
            request.Username, cancellationToken);

        var passwordHashResult = await ValidateAndHashPasswordAsync(
            request.Password, cancellationToken);

        var emailResult = await ValidateAndCreateEmailAsync(
            request.Email, cancellationToken);

        var fullNameResult = FullName.Create(request.FirstName, request.LastName);

        var gender = (Gender)request.Gender;

        var birthDate = request.BirthDate;

        return await (await Result.Combine(
            usernameResult,
            passwordHashResult,
            emailResult,
            fullNameResult)
            .OnSuccess(() => User.Create(
                usernameResult.ValueOrDefault(),
                passwordHashResult.ValueOrDefault(),
                emailResult.ValueOrDefault(),
                fullNameResult.ValueOrDefault(),
                gender,
                UserRole.User,
                birthDate))
            .EnsureAsync(
            async u => await _userRepository.AddAsync(u, cancellationToken),
            ApplicationErrors.User.AddingToDatabaseFailed))
            .OnSuccessAsync(async u => await SendRegistrationSuccessEmailAsync(u));
    }

    private async Task<Result<Username>> ValidateAndCreateUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        return await Username.Create(username)
            .EnsureAsync(
            async u => (await _userRepository.IsUsernameUnique(u, cancellationToken)).ValueOrDefault(),
            ApplicationErrors.Username.AlreadyExists);
    }

    private async Task<Result<PasswordHash>> ValidateAndHashPasswordAsync(
        string password,
        CancellationToken cancellationToken = default)
    {
        return await Password.Create(password)
            .MapAsync(async p => await _passwordHasher.HashAsync(p, cancellationToken));
    }

    private async Task<Result<Email>> ValidateAndCreateEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await Email.Create(email)
            .EnsureAsync(
            async e => (await _userRepository.IsEmailUnique(e, cancellationToken)).ValueOrDefault(),
            ApplicationErrors.Email.AlreadyExists);
    }

    private async Task<Result> SendRegistrationSuccessEmailAsync(User recipient)
    {
       EmailMessage eMsg = new EmailMessage(
           Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
           recipient.Email,
           "WorkoutTracker: Registration was successful",
           $"Hey {recipient.Username.Login}!\n\n" +
           $"You have registered a new WorkoutTracker account, now you can use the web-app fully!\n" +
           "We're glad you've chosen our application to manage your workout routines!\n\n" +
           "Good luck with your progress and remember: NEVER skip leg day!");

        return Result.Ensure(
            (await _emailService.SendEmailAsync(eMsg)).IsSuccess,
            ApplicationErrors.Email.SendingFailed);
    }
}
