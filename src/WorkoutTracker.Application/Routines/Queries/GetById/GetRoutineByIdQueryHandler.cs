namespace WorkoutTracker.Application.Routines.Queries.GetById;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.Results;

public sealed class GetRoutineByIdQueryHandler(
    IRoutineRepository routineRepository)
    : IQueryHandler<GetRoutineByIdQuery, RoutineResponse>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;

    public async Task<Result<RoutineResponse>> Handle(
        GetRoutineByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var routineResult = await RoutineId.FromGuid(request.Id)
            .MapAsync(async id => await _routineRepository.GetByIdAsync(id, cancellationToken));

        return routineResult.Map(e => new RoutineResponse
        {
            Id = e.Id.IdValue,
            Name = e.Name.Value,
            Description = e.Description.Text ?? string.Empty,
            UserId = e.UserId.IdValue
        });
    }
}
