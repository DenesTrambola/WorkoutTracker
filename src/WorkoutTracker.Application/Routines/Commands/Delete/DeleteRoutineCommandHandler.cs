namespace WorkoutTracker.Application.Routines.Commands.Delete;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Routines.Errors;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed class DeleteRoutineCommandHandler(
    IRoutineRepository routineRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteRoutineCommand>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeleteRoutineCommand request,
        CancellationToken cancellationToken = default)
    {
        var routineIdResult = RoutineId.FromGuid(request.Id);

        var routineResult = await routineIdResult.MapAsync(
            async id => await _routineRepository.GetByIdAsync(id, cancellationToken));

        var deleteResult = await routineResult.OnSuccessAsync(
            async e => await _routineRepository.DeleteAsync(routineIdResult.ValueOrDefault()));

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Routine.CannotDeleteFromDatabase);
        }

        return deleteResult;
    }
}
