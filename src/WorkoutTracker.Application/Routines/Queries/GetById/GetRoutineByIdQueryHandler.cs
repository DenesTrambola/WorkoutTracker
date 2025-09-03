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

        return routineResult.Map(r => new RoutineResponse
        {
            Id = r.Id.IdValue,
            Name = r.Name.Value,
            Description = r.Description.Text ?? string.Empty,
            UserId = r.UserId.IdValue
        });
    }
}
