namespace WorkoutTracker.Application.Users.Commands.Update;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Application.Users.Primitives;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken = default)
    {
        var userResult = await (await TryGetUserByIdAsync(request.Id, cancellationToken))
            .MapAsync(async u =>
            {
                return Result.Combine(
                    await TryUpdateUsernameAsync(u, request.Username, cancellationToken),
                    await TryUpdatePasswordAsync(u, request.Password, cancellationToken),
                    await TryUpdateEmailAsync(u, request.Email, cancellationToken),
                    TryUpdateFirstName(u, request.FirstName, cancellationToken),
                    TryUpdateLastName(u, request.LastName, cancellationToken),
                    TryUpdateGender(u, request.Gender, cancellationToken),
                    TryUpdateRole(u, request.Role, cancellationToken),
                    TryUpdateBirthDate(u, request.BirthDate, cancellationToken));
            });

        if (userResult.IsFailure)
            return Result.Failure(userResult.Errors);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.User.CannotUpdateInDatabase);
        }

        return userResult;
    }

    private async Task<Result<User>> TryGetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await UserId.FromGuid(userId)
            .MapAsync(async id => await _userRepository.GetByIdAsync(id, cancellationToken));
    }

    private async Task<Result<User>> TryUpdateUsernameAsync(
        User user,
        string? newUsername,
        CancellationToken cancellationToken = default)
    {
        return newUsername is null
            ? user
            : (await Username.Create(newUsername)
            .OnSuccessAsync(async un => await _userRepository.ValidateUsernameUniqueness(un)))
            .Map(user.UpdateUsername);
    }

    private async Task<Result<User>> TryUpdatePasswordAsync(
        User user,
        string? newPassword,
        CancellationToken cancellationToken = default)
    {
        return newPassword is null
            ? user
            : (await Password.Create(newPassword)
            .MapAsync(async p => await _passwordHasher.HashAsync(p, cancellationToken)))
            .Map(user.UpdatePasswordHash);
    }

    private async Task<Result<User>> TryUpdateEmailAsync(
        User user,
        string? newEmail,
        CancellationToken cancellationToken = default)
    {
        return newEmail is null
            ? user
            : (await Email.Create(newEmail)
            .OnSuccessAsync(async e => await _userRepository.ValidateEmailUniqueness(e)))
            .Map(user.UpdateEmail);
    }

    private Result<User> TryUpdateFirstName(
        User user,
        string? newFirstName,
        CancellationToken cancellationToken = default)
    {
        return newFirstName is null
            ? user
            : FullName.Create(newFirstName, user.FullName.LastName)
            .Map(user.UpdateFullName);
    }

    private Result<User> TryUpdateLastName(
        User user,
        string? newLastName,
        CancellationToken cancellationToken = default)
    {
        return newLastName is null
            ? user
            : FullName.Create(user.FullName.FirstName, newLastName)
            .Map(user.UpdateFullName);
    }

    private Result<User> TryUpdateGender(
        User user,
        byte? newGender,
        CancellationToken cancellationToken = default)
    {
        return newGender is null
            ? user
            : user.UpdateGender((Gender)newGender);
    }

    private Result<User> TryUpdateRole(
        User user,
        UserRole? newRole,
        CancellationToken cancellationToken = default)
    {
        return newRole is null
            ? user
            : user.UpdateRole(newRole.Value);
    }

    private Result<User> TryUpdateBirthDate(
        User user,
        DateOnly? newBirthDate,
        CancellationToken cancellationToken = default)
    {
        return newBirthDate is null
            ? user
            : user.UpdateBirthDate(newBirthDate.Value);
    }
}
