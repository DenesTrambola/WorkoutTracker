namespace WorkoutTracker.Application.Users.Queries.GetAll;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;

public sealed class GetAllUsersQueryHandler(
    IUserRepository userRepository)
    : IQueryHandler<GetAllUsersQuery, IEnumerable<UserResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<IEnumerable<UserResponse>>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken = default)
    {
        var usersResult = await _userRepository.GetAllAsync(cancellationToken);

        if (request.Username is not null)
            usersResult = usersResult.Map(u => u.Where(
                u => u.Username.Value == request.Username));

        if (request.Email is not null)
            usersResult = usersResult.Map(u => u.Where(
                u => u.Email.Value == request.Email));

        if (request.Gender is not null)
            usersResult = usersResult.Map(u => u.Where(
                u => (byte)u.Gender == request.Gender));

        if (request.Role is not null)
            usersResult = usersResult.Map(u => u.Where(
                u => u.Role == request.Role));

        if (request.BirthDate is not null)
            usersResult = usersResult.Map(u => u.Where(
                u => u.BirthDate == request.BirthDate));

        if (request.CreatedOn is not null)
            usersResult = usersResult.Map(u => u.Where(
                u => u.CreatedOn == request.CreatedOn));

        return usersResult.Map(r => r.Select(u => new UserResponse
        {
            Id = u.Id.IdValue,
            Username = u.Username.Value,
            Email = u.Email.Value,
            Gender = (byte)u.Gender,
            Role = u.Role,
            BirthDate = u.BirthDate,
            CreatedOn = u.CreatedOn
        }));
    }
}
