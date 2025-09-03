namespace WorkoutTracker.Application.Users.Commands.UpdateWorkout;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class UpdateWorkoutCommandHandler(
    IUserRepository userRepository,
    IRoutineRepository routineRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateWorkoutCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateWorkoutCommand request,
        CancellationToken cancellationToken = default)
    {
        var workoutResult = (await TryGetWorkoutByIdAsync(request.Id, cancellationToken))
            .Map(w =>
            {
                return Result.Combine(
                    TryUpdateStartTime(w, request.StartTime, cancellationToken),
                    TryUpdateEndTime(w, request.EndTime, cancellationToken),
                    TryUpdateComment(w, request.Comment, cancellationToken),
                    TryReassignToUser(w, request.UserId, cancellationToken),
                    TryReassignToRoutine(w, request.RoutineId, cancellationToken));
            });

        if (workoutResult.IsFailure)
            return workoutResult;

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Workout.CannotUpdateInDatabase);
        }

        return workoutResult;
    }

    private async Task<Result<Workout>> TryGetWorkoutByIdAsync(
        Guid workoutId,
        CancellationToken cancellationToken = default)
    {
        return await WorkoutId.FromGuid(workoutId)
            .MapAsync(async id => await _userRepository.GetWorkoutByIdAsync(id, cancellationToken));
    }

    private Result<Workout> TryUpdateStartTime(
        Workout workout,
        DateTime? newStartTime,
        CancellationToken cancellationToken = default)
    {
        return newStartTime is null
            ? workout
            : workout.UpdateStartTime(newStartTime.Value);
    }

    private Result<Workout> TryUpdateEndTime(
        Workout workout,
        DateTime? newEndTime,
        CancellationToken cancellationToken = default)
    {
        return newEndTime is null
            ? workout
            : workout.UpdateEndTime(newEndTime.Value);
    }

    private Result<Workout> TryUpdateComment(
        Workout workout,
        string? newComment,
        CancellationToken cancellationToken = default)
    {
        return newComment is null
            ? workout
            : Comment.Create(newComment)
            .Map(workout.UpdateComment);
    }

    private Result<Workout> TryReassignToUser(
        Workout workout,
        Guid? newUserId,
        CancellationToken cancellationToken = default)
    {
        return newUserId is null
            ? workout
            : UserId.FromGuid(newUserId.Value)
            .Map(workout.ReassignToUser);
    }

    private Result<Workout> TryReassignToRoutine(
        Workout workout,
        Guid? newRoutineId,
        CancellationToken? cancellationToken = default)
    {
        return newRoutineId is null
            ? workout
            : RoutineId.FromGuid(newRoutineId.Value)
            .Map(workout.ReassignToRoutine);
    }
}
