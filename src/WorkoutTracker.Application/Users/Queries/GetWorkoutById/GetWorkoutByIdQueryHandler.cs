namespace WorkoutTracker.Application.Users.Queries.GetWorkoutById;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class GetWorkoutByIdQueryHandler(
    IUserRepository userRepository)
    : IQueryHandler<GetWorkoutByIdQuery, WorkoutResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<WorkoutResponse>> Handle(
        GetWorkoutByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var workoutResult = await WorkoutId.FromGuid(request.Id)
            .MapAsync(async id => await _userRepository.GetWorkoutByIdAsync(id, cancellationToken));

        return workoutResult.Map(w => new WorkoutResponse
        {
            Id = w.Id.IdValue,
            StartTime = w.StartTime,
            EndTime = w.EndTime,
            Duration = w.Duration,
            RestTimeBetweenExercises = w.RestTimeBetweenExercises,
            Comment = w.Comment.Text ?? string.Empty,
            UserId = w.UserId.IdValue,
            RoutineId = w.RoutineId.IdValue
        });
    }
}
