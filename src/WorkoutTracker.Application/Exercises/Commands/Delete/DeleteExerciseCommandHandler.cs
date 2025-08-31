namespace WorkoutTracker.Application.Exercises.Commands.Delete;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Exercises.Errors;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed class DeleteExerciseCommandHandler(
    IExerciseRepository exerciseRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteExerciseCommand>
{
    private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeleteExerciseCommand request,
        CancellationToken cancellationToken = default)
    {
        var exerciseIdResult = ExerciseId.FromGuid(request.Id);

        var exerciseResult = await exerciseIdResult.MapAsync(
            async id => await _exerciseRepository.GetByIdAsync(id, cancellationToken));

        var deleteResult = await exerciseResult.OnSuccessAsync(
            async e => await _exerciseRepository.DeleteAsync(exerciseIdResult.ValueOrDefault()));

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Exercise.CannotDeleteFromDatabase);
        }

        return deleteResult;
    }
}
