namespace WorkoutTracker.Application.Routines.Commands.DeleteExercise;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Routines.Errors;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed class DeleteRoutineExerciseCommandHandler(
    IRoutineRepository routineRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteRoutineExerciseCommand>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeleteRoutineExerciseCommand request,
        CancellationToken cancellationToken = default)
    {
        var routineExerciseId = RoutineExerciseId.FromGuid(request.Id);

        var routineExerciseResult = await routineExerciseId.MapAsync(
            async id => await _routineRepository.GetExerciseByIdAsync(id, cancellationToken));

        var deleteResult = await routineExerciseResult.OnSuccessAsync(
            async re => await _routineRepository.DeleteExerciseAsync(
                routineExerciseId.ValueOrDefault(),cancellationToken));

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.RoutineExercise.CannotDeleteFromDatabase);
        }

        return deleteResult;
    }
}
