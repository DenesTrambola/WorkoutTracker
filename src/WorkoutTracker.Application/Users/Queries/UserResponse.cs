namespace WorkoutTracker.Application.Users.Queries;

using WorkoutTracker.Domain.Users.Enums;

public sealed record UserResponse
{
    public required Guid Id { get; init; }

    public required string Username { get; init; }

    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required byte Gender { get; init; }

    public required UserRole Role { get; init; }

    public required DateOnly BirthDate { get; init; }

    public required DateTime CreatedOn { get; init; }
}
