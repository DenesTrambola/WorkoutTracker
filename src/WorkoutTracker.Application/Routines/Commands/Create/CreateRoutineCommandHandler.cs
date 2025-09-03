namespace WorkoutTracker.Application.Routines.Commands.Create;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Routines.Errors;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class CreateRoutineCommandHandler(
    IRoutineRepository routineRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoutineCommand>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        CreateRoutineCommand request,
        CancellationToken cancellationToken = default)
    {
        var nameResult = Name.Create(request.Name);
        var descriptionResult = Description.Create(request.Description);
        var userIdResult = await ValidateUserIdAsync(request.UserId, cancellationToken);

        var routineResult = await Result.Combine(
            nameResult, descriptionResult, userIdResult)
            .OnSuccess(() => Routine.Create(
                nameResult.ValueOrDefault(),
                descriptionResult.ValueOrDefault(),
                userIdResult.ValueOrDefault()))
            .OnSuccessAsync(async r => await _routineRepository.AddAsync(r));

        if (routineResult.IsFailure)
            return routineResult;

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Routine.CannotAddToDatabase);
        }

        return routineResult;
    }

    private async Task<Result<UserId>> ValidateUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userResult = await UserId.FromGuid(userId).MapAsync(
            async uId => await _userRepository.GetByIdAsync(uId));

        return userResult.Map(u => u.Id);
    }
}
