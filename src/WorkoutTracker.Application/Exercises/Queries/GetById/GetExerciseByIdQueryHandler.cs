namespace WorkoutTracker.Application.Exercises.Queries.GetById;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Shared.Results;

public sealed class GetExerciseByIdQueryHandler(
    IExerciseRepository exerciseRepository)
    : IQueryHandler<GetExerciseByIdQuery, ExerciseResponse>
{
    private readonly IExerciseRepository _exerciseRepository = exerciseRepository;

    public async Task<Result<ExerciseResponse>> Handle(
        GetExerciseByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var exerciseResult = await ExerciseId.FromGuid(request.Id)
            .MapAsync(async id => await _exerciseRepository.GetByIdAsync(id, cancellationToken));

        return exerciseResult.Map(e => new ExerciseResponse
        {
            Id = e.Id.IdValue,
            Name = e.Name.Value,
            TargetMuscle = e.TargetMuscle.Muscle,
            IsPublic = e.Visibility.IsPublic,
            UserId = e.UserId.IdValue
        });
    }
}
