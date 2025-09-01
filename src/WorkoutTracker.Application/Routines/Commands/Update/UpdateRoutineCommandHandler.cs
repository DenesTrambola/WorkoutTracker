namespace WorkoutTracker.Application.Routines.Commands.Update;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Routines.Errors;
using WorkoutTracker.Application.Routines.Queries;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class UpdateRoutineCommandHandler(
    IRoutineRepository routineRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateRoutineCommand, RoutineResponse>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<RoutineResponse>> Handle(
        UpdateRoutineCommand request,
        CancellationToken cancellationToken = default)
    {
        var routineResult = (await TryGetRoutineByIdAsync(request.Id, cancellationToken))
            .Map(r =>
            {
                return Result.Combine(
                    TryUpdateName(r, request.Name, cancellationToken),
                    TryUpdateDescription(r, request.Description, cancellationToken),
                    TryReassignToUser(r, request.UserId, cancellationToken));
            });

        if (routineResult.IsFailure)
            return Result.Failure<RoutineResponse>(routineResult.Errors);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure<RoutineResponse>(ApplicationErrors.Routine.CannotUpdateInDatabase);
        }

        return routineResult.Map(r => new RoutineResponse
        {
            Id = r.Id.IdValue,
            Name = r.Name.Value,
            Description = r.Description.Text ?? string.Empty,
            UserId = r.UserId.IdValue
        });
    }

    private async Task<Result<Routine>> TryGetRoutineByIdAsync(
        Guid routineId,
        CancellationToken cancellationToken = default)
    {
        return await RoutineId.FromGuid(routineId)
            .MapAsync(async id => await _routineRepository.GetByIdAsync(id, cancellationToken));
    }

    private Result<Routine> TryUpdateName(
        Routine routine,
        string? newName,
        CancellationToken cancellationToken = default)
    {
        return newName is null
            ? routine
            : Name.Create(newName)
            .Map(routine.UpdateName);
    }

    private Result<Routine> TryUpdateDescription(
        Routine routine,
        string? newDescription,
        CancellationToken cancellationToken = default)
    {
        return newDescription is null
            ? routine
            : Description.Create(newDescription)
            .Map(routine.UpdateDescription);
    }

    private Result<Routine> TryReassignToUser(
        Routine routine,
        Guid? newUserId,
        CancellationToken cancellationToken = default)
    {
        return newUserId is null || !newUserId.HasValue
            ? routine
            : UserId.FromGuid(newUserId!.Value)
            .Map(routine.ReassignToUser);
    }
}
