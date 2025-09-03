namespace WorkoutTracker.Application.Routines.Queries.GetAllExercises;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Shared.Results;

public sealed class GetAllRoutineExercisesQueryHandler(
    IRoutineRepository routineRepository,
    IExerciseRepository exerciseRepository)
    : IQueryHandler<GetAllRoutineExercisesQuery, IEnumerable<RoutineExerciseResponse>>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IExerciseRepository _exerciseRepository = exerciseRepository;

    public async Task<Result<IEnumerable<RoutineExerciseResponse>>> Handle(
        GetAllRoutineExercisesQuery request,
        CancellationToken cancellationToken = default)
    {
        var exercisesResult = await _routineRepository.GetAllExercisesAsync(cancellationToken);

        if (request.SetCount is not null)
            exercisesResult = exercisesResult.Map(re => re.Where(
                re => re.SetCount == request.SetCount));

        if (request.RepCount is not null)
            exercisesResult = exercisesResult.Map(re => re.Where(
                re => re.RepCount == request.RepCount));

        if (request.RestTimeBetweenSets is not null)
            exercisesResult = exercisesResult.Map(re => re.Where(
                re => re.RestTimeBetweenSets == request.RestTimeBetweenSets));

        if (request.Comment is not null)
            exercisesResult = exercisesResult.Map(re => re.Where(
                re => re.Comment.Text == request.Comment));

        if (request.Position is not null)
            exercisesResult = exercisesResult.Map(re => re.Where(
                re => re.Position.Value == request.Position));

        if (request.RoutineId is not null)
            exercisesResult = exercisesResult.Map(re => re.Where(
                re => re.RoutineId.IdValue == request.RoutineId));

        if (request.ExerciseId is not null)
            exercisesResult = exercisesResult.Map(re => re.Where(
                re => re.ExerciseId.IdValue == request.ExerciseId));

        return exercisesResult.Map(re => re.Select(re => new RoutineExerciseResponse
        {
            Id = re.Id.IdValue,
            SetCount = re.SetCount,
            RepCount = re.RepCount,
            RestTimeBetweenSets = re.RestTimeBetweenSets,
            Comment = re.Comment.Text ?? string.Empty,
            Position = re.Position.Value,
            RoutineId = re.RoutineId.IdValue,
            ExerciseId = re.ExerciseId.IdValue
        }));
    }
}
