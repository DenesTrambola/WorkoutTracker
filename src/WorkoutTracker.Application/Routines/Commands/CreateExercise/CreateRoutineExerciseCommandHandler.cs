namespace WorkoutTracker.Application.Routines.Commands.CreateExercise;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Routines.Errors;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Routines.ValueObjects;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;

public sealed class CreateRoutineExerciseCommandHandler(
    IRoutineRepository routineRepository,
    IExerciseRepository exerciseRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoutineExerciseCommand>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IExerciseRepository _exerciseRepository = exerciseRepository;

    public async Task<Result> Handle(CreateRoutineExerciseCommand request, CancellationToken cancellationToken)
    {
        var setCount = request.SetCount;
        var repCount = request.RepCount;
        var restTime = request.RestTimeBetweenSets;
        var commentResult = Comment.Create(request.Comment);
        var routineIdResult = await ValidateRoutineIdAsync(request.RoutineId, cancellationToken);
        var exerciseIdResult = await ValidateExerciseIdAsync(request.ExerciseId, cancellationToken);

        var positionResult = await routineIdResult
            .MapAsync(async rId => await TryCreatePositionAsync(rId, cancellationToken));
        var routineResult = await routineIdResult
            .MapAsync(async rId => await _routineRepository.GetByIdAsync(rId, cancellationToken));

        var routineExerciseResult = await Result.Combine(
            commentResult, positionResult, routineResult)
            .OnSuccess(() => routineResult.ValueOrDefault().AddExercise(
                setCount, repCount, restTime,
                commentResult.ValueOrDefault(),
                positionResult.ValueOrDefault(),
                exerciseIdResult.ValueOrDefault()))
            .OnSuccessAsync(async re => await _routineRepository.AddExerciseAsync(re, cancellationToken));

        if (routineExerciseResult.IsFailure)
            return routineExerciseResult;

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.RoutineExercise.CannotAddToDatabase);
        }

        return routineExerciseResult;
    }

    private async Task<Result<ExercisePosition>> TryCreatePositionAsync(
        RoutineId routineId,
        CancellationToken cancellationToken = default)
    {
        return (await _routineRepository.GetAllExercisesByRoutineIdAsync(routineId, cancellationToken))
            .Map(reList =>
            {
                var position = (byte)(reList.Count() + 1);
                return ExercisePosition.Create(position);
            });
    }

    private async Task<Result<RoutineId>> ValidateRoutineIdAsync(
        Guid routineId,
        CancellationToken cancellationToken = default)
    {
        return (await RoutineId.FromGuid(routineId)
            .MapAsync(async rId => await _routineRepository.GetByIdAsync(rId, cancellationToken)))
            .Map(r => r.Id);
    }

    private async Task<Result<ExerciseId>> ValidateExerciseIdAsync(
        Guid exerciseId,
        CancellationToken cancellationToken = default)
    {
        return (await ExerciseId.FromGuid(exerciseId)
            .MapAsync(async eId => await _exerciseRepository.GetByIdAsync(eId, cancellationToken)))
            .Map(e => e.Id);
    }
}
