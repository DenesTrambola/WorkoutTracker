namespace WorkoutTracker.Application.Users.Queries.GetAllWorkouts;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;

public sealed record GetAllWorkoutsQueryHandler(
    IUserRepository userRepository,
    IRoutineRepository routineRepository)
    : IQueryHandler<GetAllWorkoutsQuery, IEnumerable<WorkoutResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoutineRepository _routineRepository = routineRepository;

    public async Task<Result<IEnumerable<WorkoutResponse>>> Handle(
        GetAllWorkoutsQuery request,
        CancellationToken cancellationToken = default)
    {
        var workoutsResult = await _userRepository.GetAllWorkoutsAsync(cancellationToken);

        if (request.StartTime is not null)
            workoutsResult = workoutsResult.Map(w => w.Where(
                w => w.StartTime == request.StartTime));

        if (request.EndTime is not null)
            workoutsResult = workoutsResult.Map(w => w.Where(
                w => w.EndTime == request.EndTime));

        if (request.Duration is not null)
            workoutsResult = workoutsResult.Map(w => w.Where(
                w => w.Duration == request.Duration));

        if (request.RestTimeBetweenExercises is not null)
            workoutsResult = workoutsResult.Map(w => w.Where(
                w => w.RestTimeBetweenExercises == request.RestTimeBetweenExercises));

        if (request.Comment is not null)
            workoutsResult = workoutsResult.Map(w => w.Where(
                w => w.Comment.Text == request.Comment));

        if (request.UserId is not null)
            workoutsResult = workoutsResult.Map(w => w.Where(
                w => w.UserId.IdValue == request.UserId));

        if (request.RoutineId is not null)
            workoutsResult = workoutsResult.Map(w => w.Where(
                w => w.RoutineId.IdValue == request.RoutineId));

        return workoutsResult.Map(w => w.Select(w => new WorkoutResponse
        {
            Id = w.Id.IdValue,
            StartTime = w.StartTime,
            EndTime = w.EndTime,
            Duration = w.Duration,
            RestTimeBetweenExercises = w.RestTimeBetweenExercises,
            Comment = w.Comment.Text ?? string.Empty,
            UserId = w.UserId.IdValue,
            RoutineId = w.RoutineId.IdValue
        }));
    }
}
