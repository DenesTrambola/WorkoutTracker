namespace WorkoutTracker.Application.Exercises.Commands.Create;

using System;
using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Exercises.Errors;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class CreateExerciseCommandHandler(
    IExerciseRepository exerciseRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateExerciseCommand>
{
    private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        CreateExerciseCommand request,
        CancellationToken cancellationToken = default)
    {
        var nameResult = Name.Create(request.Name);
        var targetMuscleResult = TargetMuscle.Create(request.TargetMuscle);
        var visibilityResult = Visibility.Create(request.IsPublic);
        var userIdResult = await ValidateUserIdAsync(request.UserId);

        var exerciseResult = await Result.Combine(
            nameResult, visibilityResult, targetMuscleResult, userIdResult)
            .OnSuccess(() => Exercise.Create(
                nameResult.ValueOrDefault(),
                targetMuscleResult.ValueOrDefault(),
                visibilityResult.ValueOrDefault(),
                userIdResult.ValueOrDefault()))
            .OnSuccessAsync(async e => await _exerciseRepository.AddAsync(e));

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Exercise.CannotAddToDatabase);
        }

        return exerciseResult;
    }

    private async Task<Result<UserId>> ValidateUserIdAsync(Guid userId)
    {
        var userResult = await UserId.FromGuid(userId).MapAsync(
            async uId => await _userRepository.GetByIdAsync(uId));

        return userResult.Map(u => u.Id);
    }
}
