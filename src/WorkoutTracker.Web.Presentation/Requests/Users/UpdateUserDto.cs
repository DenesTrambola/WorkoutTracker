namespace WorkoutTracker.Web.Presentation.Requests.Users;

using WorkoutTracker.Domain.Users.Enums;

public sealed record UpdateUserDto
{
    public string? Username { get; init; }

    public string? Password { get; init; }

    public string? Email { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public byte? Gender { get; init; }

    public UserRole? Role { get; init; }

    public DateOnly? BirthDate { get; init; }
}
