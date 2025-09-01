namespace WorkoutTracker.Application.Users.Commands.Delete;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken = default)
    {
        var userIdResult = UserId.FromGuid(request.Id);

        var userResult = await userIdResult.MapAsync(
            async id => await _userRepository.GetByIdAsync(id, cancellationToken));

        var deleteResult = await userResult.OnSuccessAsync(
            async u => await _userRepository.DeleteAsync(userIdResult.ValueOrDefault()));

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.User.CannotDeleteFromDatabase);
        }

        return deleteResult;
    }
}
