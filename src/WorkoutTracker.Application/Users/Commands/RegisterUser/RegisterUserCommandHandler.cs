namespace WorkoutTracker.Application.Users.Commands.RegisterUser;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WorkoutTracker.Application.Shared.Models;
using WorkoutTracker.Application.Shared.Primitives;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Application.Users.Primitives;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IEmailService emailService)
    : ICommandHandler<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IEmailService _emailService = emailService;

    public async Task<Result> Handle(
        [NotNull] RegisterUserCommand request,
        CancellationToken cancellationToken = default)
    {
        var usernameResult = await CreateAndValidateUsernameAsync(
            request.Username, cancellationToken);

        var passwordHashResult = await ValidateAndHashPasswordAsync(
            request.Password, cancellationToken);

        var emailResult = await CreateAndValidateEmailAsync(
            request.Email, cancellationToken);

        var fullNameResult = FullName.Create(request.FirstName, request.LastName);

        var gender = (Gender)request.Gender;

        var birthDate = request.BirthDate;

        var userResult = await (await Result.Combine(
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
            .OnSuccessAsync(async u => await _userRepository.AddAsync(u, cancellationToken)))
            .OnSuccessAsync(async u => await SendRegistrationSuccessEmail(u));

        if (userResult.IsFailure)
            return userResult;

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.User.CannotAddToDatabase);
        }

        return userResult;
    }

    private async Task<Result<Username>> CreateAndValidateUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        return await Username.Create(username).OnSuccessAsync(
            async u => await _userRepository.ValidateUsernameUniqueness(u, cancellationToken));
    }

    private async Task<Result<PasswordHash>> ValidateAndHashPasswordAsync(
        string password,
        CancellationToken cancellationToken = default)
    {
        return await Password.Create(password).MapAsync(
            async p => await _passwordHasher.HashAsync(p, cancellationToken));
    }

    private async Task<Result<Email>> CreateAndValidateEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await Email.Create(email).OnSuccessAsync(
            async e => await _userRepository.ValidateEmailUniqueness(e, cancellationToken));
    }

    private Task<Result> SendRegistrationSuccessEmail(User recipient)
    {
        var eMsg = new EmailMessage
        {
            From = Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            To = recipient.Email,
            Subject = "WorkoutTracker: Registration was successful",
            Body =
            $"Hey {recipient.Username.Value}!\n\n" +
            $"You have registered a new WorkoutTracker account, now you can use the web-app fully!\n" +
            "We're glad you've chosen our application to manage your workout routines!\n\n" +
            "Good luck with your progress and remember: NEVER skip leg day!",
        };

        return _emailService.SendEmailAsync(eMsg);
    }
}
