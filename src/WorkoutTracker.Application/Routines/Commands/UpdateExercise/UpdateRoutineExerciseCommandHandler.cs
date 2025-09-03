namespace WorkoutTracker.Application.Routines.Commands.UpdateExercise;

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

public sealed class UpdateRoutineExerciseCommandHandler(
    IRoutineRepository routineRepository,
    IExerciseRepository exerciseRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateRoutineExerciseCommand>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateRoutineExerciseCommand request,
        CancellationToken cancellationToken = default)
    {
        var routineExerciseResult = (await TryGetRoutineExerciseByIdAsync(request.Id, cancellationToken))
            .Map(re =>
            {
                return Result.Combine(
                    TryUpdateSetCount(re, request.SetCount, cancellationToken),
                    TryUpdateRepCount(re, request.RepCount, cancellationToken),
                    TryUpdateRestTime(re, request.RestTimeBetweenSets, cancellationToken),
                    TryUpdateComment(re, request.Comment, cancellationToken),
                    TryUpdatePosition(re, request.Position, cancellationToken),
                    TryReassignToRoutine(re, request.RoutineId, cancellationToken),
                    TryReassignToExercise(re, request.ExerciseId, cancellationToken));
            });

        if (routineExerciseResult.IsFailure)
            return routineExerciseResult;

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.RoutineExercise.CannotUpdateInDatabase);
        }

        return routineExerciseResult;
    }

    private async Task<Result<RoutineExercise>> TryGetRoutineExerciseByIdAsync(
        Guid routineExerciseId,
        CancellationToken cancellationToken = default)
    {
        return await RoutineExerciseId.FromGuid(routineExerciseId)
            .MapAsync(async id => await _routineRepository.GetExerciseByIdAsync(id, cancellationToken));
    }

    private Result<RoutineExercise> TryUpdateSetCount(
        RoutineExercise routineExercise,
        byte? newSetCount,
        CancellationToken cancellationToken = default)
    {
        return newSetCount is null
            ? routineExercise
            : routineExercise.UpdateSetCount(newSetCount.Value);
    }

    private Result<RoutineExercise> TryUpdateRepCount(
        RoutineExercise routineExercise,
        byte? newRepCount,
        CancellationToken cancellationToken = default)
    {
        return newRepCount is null
            ? routineExercise
            : routineExercise.UpdateRepCount(newRepCount.Value);
    }

    private Result<RoutineExercise> TryUpdateRestTime(
        RoutineExercise routineExercise,
        TimeSpan? newRestTime,
        CancellationToken cancellationToken = default)
    {
        return newRestTime is null
            ? routineExercise
            : routineExercise.UpdateRestTimeBetweenSets(newRestTime.Value);
    }

    private Result<RoutineExercise> TryUpdateComment(
        RoutineExercise routineExercise,
        string? newComment,
        CancellationToken cancellationToken = default)
    {
        return newComment is null
            ? routineExercise
            : Comment.Create(newComment)
            .Map(routineExercise.UpdateComment);
    }

    private Result<RoutineExercise> TryUpdatePosition(
        RoutineExercise routineExercise,
        byte? newPosition,
        CancellationToken cancellationToken = default)
    {
        return newPosition is null
            ? routineExercise
            : ExercisePosition.Create(newPosition.Value)
            .Map(routineExercise.UpdatePosition);
    }

    private Result<RoutineExercise> TryReassignToRoutine(
        RoutineExercise routineExercise,
        Guid? newRoutineId,
        CancellationToken cancellationToken = default)
    {
        return newRoutineId is null
            ? routineExercise
            : RoutineId.FromGuid(newRoutineId.Value)
            .Map(routineExercise.ReassignToRoutine);
    }

    private Result<RoutineExercise> TryReassignToExercise(
        RoutineExercise routineExercise,
        Guid? newExerciseId,
        CancellationToken cancellationToken = default)
    {
        return newExerciseId is null
            ? routineExercise
            : ExerciseId.FromGuid(newExerciseId.Value)
            .Map(routineExercise.ReassignToExercise);
    }
}
