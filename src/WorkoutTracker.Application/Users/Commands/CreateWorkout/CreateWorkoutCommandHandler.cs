namespace WorkoutTracker.Application.Users.Commands.CreateWorkout;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Application.Users.Commands.AddWorkoutToUser;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class CreateWorkoutCommandHandler(
    IUserRepository userRepository,
    IRoutineRepository routineRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateWorkoutCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        CreateWorkoutCommand request,
        CancellationToken cancellationToken = default)
    {
        var startTime = request.StartTime;
        var endTime = request.EndTime;
        var restTime = request.RestTimeBetweenExercises;
        var commentResult = Comment.Create(request.Comment);
        var routineIdResult = await ValidateRoutineIdAsync(request.RoutineId, cancellationToken);
        var userIdResult = await ValidateUserIdAsync(request.UserId, cancellationToken);
        var userResult = await userIdResult.MapAsync(
            async uId => await _userRepository.GetByIdAsync(uId, cancellationToken));

        var workoutResult = await Result.Combine(
            commentResult, routineIdResult, userResult)
            .OnSuccess(() => userResult.ValueOrDefault().AddWorkout(
                startTime, endTime, restTime,
                commentResult.ValueOrDefault(),
                routineIdResult.ValueOrDefault()))
            .OnSuccessAsync(async w => await _userRepository.AddWorkoutAsync(w, cancellationToken));

        if (workoutResult.IsFailure)
            return workoutResult;

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Workout.CannotAddToDatabase);
        }

        return workoutResult;
    }

    private async Task<Result<UserId>> ValidateUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return (await UserId.FromGuid(userId)
            .MapAsync(async uId => await _userRepository.GetByIdAsync(uId, cancellationToken)))
            .Map(u => u.Id);
    }

    private async Task<Result<RoutineId>> ValidateRoutineIdAsync(
        Guid routineId,
        CancellationToken cancellationToken = default)
    {
        return (await RoutineId.FromGuid(routineId)
            .MapAsync(async rId => await _routineRepository.GetByIdAsync(rId, cancellationToken)))
            .Map(r => r.Id);
    }
}
