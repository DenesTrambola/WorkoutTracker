namespace WorkoutTracker.Application.Routines.Commands.ReorderExercises;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed class ReorderExercisesInRoutineCommandHandler(
    IRoutineRepository routineRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ReorderExercisesInRoutineCommand>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        ReorderExercisesInRoutineCommand request,
        CancellationToken cancellationToken)
    {
        var routineResult = await RoutineId.FromGuid(request.RoutineId)
            .MapAsync(id => _routineRepository.GetByIdAsync(id, cancellationToken));

        if (routineResult.IsFailure)
            return Result.Failure(routineResult.Errors);

        var routine = routineResult.ValueOrDefault();

        var exerciseIds = request.ExerciseIdsInOrder
            .Select(RoutineExerciseId.FromGuid)
            .ToList();

        if (exerciseIds.Any(r => r.IsFailure))
            return Result.Failure(exerciseIds.SelectMany(r => r.Errors).ToArray());

        var reorderResult = routine.ReorderExercises(
            exerciseIds.Select(r => r.ValueOrDefault()).ToList());

        if (reorderResult.IsFailure)
            return Result.Failure(reorderResult.Errors);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
