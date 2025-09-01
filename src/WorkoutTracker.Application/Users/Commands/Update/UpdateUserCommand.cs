namespace WorkoutTracker.Application.Users.Commands.Update;

using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Users.Enums;

public sealed record UpdateUserCommand
    : ICommand
{
    public required Guid Id { get; init; }

    public required string? Username { get; init; }

    public string? Password { get; init; }

    public required string? Email { get; init; }

    public required string? FirstName { get; init; }

    public required string? LastName { get; init; }

    public required byte? Gender { get; init; }

    public required UserRole? Role { get; init; }

    public required DateOnly? BirthDate { get; init; }
}
