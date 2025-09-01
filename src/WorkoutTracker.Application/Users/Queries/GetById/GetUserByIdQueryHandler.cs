namespace WorkoutTracker.Application.Users.Queries.GetById;

using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class GetUserByIdQueryHandler(
    IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<UserResponse>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var userResult = await UserId.FromGuid(request.Id)
            .MapAsync(async id => await _userRepository.GetByIdAsync(id, cancellationToken));

        return userResult.Map(u => new UserResponse
        {
            Id = u.Id.IdValue,
            Username = u.Username.Value,
            Email = u.Email.Value,
            FirstName = u.FullName.FirstName,
            LastName = u.FullName.LastName,
            Gender = (byte)u.Gender,
            Role = u.Role,
            BirthDate = u.BirthDate,
            CreatedOn = u.CreatedOn
        });
    }
}
