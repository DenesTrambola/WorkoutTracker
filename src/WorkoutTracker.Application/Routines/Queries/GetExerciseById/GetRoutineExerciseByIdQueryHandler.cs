namespace WorkoutTracker.Application.Routines.Queries.GetExerciseById;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Results;

public sealed class GetRoutineExerciseByIdQueryHandler(
    IRoutineRepository routineRepository)
    : IQueryHandler<GetRoutineExerciseByIdQuery, RoutineExerciseResponse>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;

    public async Task<Result<RoutineExerciseResponse>> Handle(
        GetRoutineExerciseByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var routineResult = await RoutineExerciseId.FromGuid(request.Id)
            .MapAsync(async id => await _routineRepository.GetExerciseByIdAsync(id, cancellationToken));

        return routineResult.Map(re => new RoutineExerciseResponse
        {
            Id = re.Id.IdValue,
            SetCount = re.SetCount,
            RepCount = re.RepCount,
            RestTimeBetweenSets = re.RestTimeBetweenSets,
            Position = re.Position.Value,
            Comment = re.Comment.Text ?? string.Empty,
            RoutineId = re.RoutineId.IdValue,
            ExerciseId = re.ExerciseId.IdValue
        });
    }
}
