namespace WorkoutTracker.Application.Routines.Queries.GetAll;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Shared.Results;

public sealed class GetAllRoutinesQueryHandler(
    IRoutineRepository routineRepository)
    : IQueryHandler<GetAllRoutinesQuery, IEnumerable<RoutineResponse>>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;

    public async Task<Result<IEnumerable<RoutineResponse>>> Handle(
        GetAllRoutinesQuery request,
        CancellationToken cancellationToken = default)
    {
        var routinesResult = await _routineRepository.GetAllAsync(cancellationToken);

        if (request.Name is not null)
            routinesResult = routinesResult.Map(r => r.Where(
                r => r.Name.Value == request.Name));

        if (request.Description is not null)
            routinesResult = routinesResult.Map(r => r.Where(
                r => r.Description.Text == request.Description));

        if (request.UserId is not null)
            routinesResult = routinesResult.Map(r => r.Where(
                r => r.UserId.IdValue == request.UserId));

        return routinesResult.Map(r => r.Select(r => new RoutineResponse
        {
            Id = r.Id.IdValue,
            Name = r.Name.Value,
            Description = r.Description.Text ?? string.Empty,
            UserId = r.UserId.IdValue
        }));
    }
}
