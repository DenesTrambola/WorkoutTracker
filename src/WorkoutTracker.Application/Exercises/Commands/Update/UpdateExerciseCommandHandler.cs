namespace WorkoutTracker.Application.Exercises.Commands.Update;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Exercises.Errors;
using WorkoutTracker.Application.Exercises.Queries;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class UpdateExerciseCommandHandler(
    IExerciseRepository exerciseRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateExerciseCommand, ExerciseResponse>
{
    private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<ExerciseResponse>> Handle(
        UpdateExerciseCommand request,
        CancellationToken cancellationToken = default)
    {
        var exerciseResult = (await TryGetExerciseByIdAsync(request.Id, cancellationToken))
            .Map(e =>
            {
                return Result.Combine(
                    TryUpdateName(e, request.Name, cancellationToken),
                    TryUpdateTargetMuscle(e, request.TargetMuscle, cancellationToken),
                    TryUpdateVisibility(e, request.IsPublic, cancellationToken),
                    TryReassignToUser(e, request.UserId, cancellationToken));
            });

        if (exerciseResult.IsFailure)
            return Result.Failure<ExerciseResponse>(exerciseResult.Errors);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure<ExerciseResponse>(ApplicationErrors.Exercise.CannotUpdateInDatabase);
        }

        return exerciseResult.Map(e => new ExerciseResponse
        {
            Id = e.Id.IdValue,
            Name = e.Name.Value,
            TargetMuscle = e.TargetMuscle.Muscle,
            IsPublic = e.Visibility.IsPublic,
            UserId = e.UserId.IdValue
        });
    }

    private async Task<Result<Exercise>> TryGetExerciseByIdAsync(
        Guid exerciseId,
        CancellationToken cancellationToken = default)
    {
        return await ExerciseId.FromGuid(exerciseId)
            .MapAsync(async id => await _exerciseRepository.GetByIdAsync(id, cancellationToken));
    }

    private Result<Exercise> TryUpdateName(
        Exercise exercise,
        string? newName,
        CancellationToken cancellationToken = default)
    {
        return newName is null
            ? exercise
            : Name.Create(newName)
            .Map(exercise.UpdateName);
    }

    private Result<Exercise> TryUpdateTargetMuscle(
        Exercise exercise,
        string? newTargetMuscle,
        CancellationToken cancellationToken = default)
    {
        return newTargetMuscle is null
            ? exercise
            : TargetMuscle.Create(newTargetMuscle)
            .Map(exercise.UpdateTargetMuscle);
    }

    private Result<Exercise> TryUpdateVisibility(
        Exercise exercise,
        bool? newIsPublic,
        CancellationToken cancellationToken = default)
    {
        return newIsPublic is null
            ? exercise
            : Visibility.Create(newIsPublic.Value)
            .Map(exercise.UpdateVisibility);
    }

    private Result<Exercise> TryReassignToUser(
        Exercise exercise,
        Guid? newUserId,
        CancellationToken cancellationToken = default)
    {
        return newUserId is null || !newUserId.HasValue
            ? exercise
            : UserId.FromGuid(newUserId!.Value)
            .Map(exercise.ReassignToUser);
    }
}
