namespace WorkoutTracker.Application.Users.Commands.DeleteWorkout;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class DeleteWorkoutCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteWorkoutCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workoutId = WorkoutId.FromGuid(request.Id);

        var workoutResult = await workoutId.MapAsync(
            async id => await _userRepository.GetWorkoutByIdAsync(id, cancellationToken));

        var deleteResult = await workoutResult.OnSuccessAsync(
            async w => await _userRepository.DeleteWorkoutAsync(
                workoutId.ValueOrDefault(), cancellationToken));

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Workout.CannotDeleteFromDatabase);
        }

        return deleteResult;
    }
}
