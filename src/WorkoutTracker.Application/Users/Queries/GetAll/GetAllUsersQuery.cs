namespace WorkoutTracker.Application.Users.Queries.GetAll;

using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Users.Enums;

public sealed record GetAllUsersQuery
    : IQuery<IEnumerable<UserResponse>>
{
    public string? Username { get; init; }

    public string? Email { get; init; }

    public byte? Gender { get; init; }

    public UserRole? Role { get; init; }

    public DateOnly? BirthDate { get; init; }

    public DateTime? CreatedOn { get; init; }
}
