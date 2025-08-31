namespace WorkoutTracker.Application.Exercises.Queries.GetAll;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Shared.Results;

public sealed class GetAllExercisesQueryHandler(
    IExerciseRepository exerciseRepository)
    : IQueryHandler<GetAllExercisesQuery, IEnumerable<ExerciseResponse>>
{
    private readonly IExerciseRepository _exerciseRepository = exerciseRepository;

    public async Task<Result<IEnumerable<ExerciseResponse>>> Handle(
        GetAllExercisesQuery request,
        CancellationToken cancellationToken = default)
    {
        var exercisesResult = await _exerciseRepository.GetAllAsync(cancellationToken);

        if (request.Name is not null)
            exercisesResult = exercisesResult.Map(e => e.Where(
                e => e.Name.Value == request.Name));

        if (request.TargetMuscle is not null)
            exercisesResult = exercisesResult.Map(e => e.Where(
                e => e.TargetMuscle.Muscle == request.TargetMuscle));

        if (request.IsPublic is not null)
            exercisesResult = exercisesResult.Map(e => e.Where(
                e => e.TargetMuscle.Muscle == request.TargetMuscle));

        if (request.UserId is not null)
            exercisesResult = exercisesResult.Map(e => e.Where(
                e => e.UserId.IdValue == request.UserId));

        return exercisesResult.Map(e => e.Select(e => new ExerciseResponse
        {
            Id = e.Id.IdValue,
            Name = e.Name.Value,
            TargetMuscle = e.TargetMuscle.Muscle,
            IsPublic = e.Visibility.IsPublic,
            UserId = e.UserId.IdValue
        }));
    }
}
