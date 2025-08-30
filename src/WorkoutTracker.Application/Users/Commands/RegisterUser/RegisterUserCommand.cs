namespace WorkoutTracker.Application.Users.Commands.RegisterUser;

using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed record RegisterUserCommand
    : ICommand
{
    public required string Username { get; init; }

    public required string Password { get; init; }

    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required byte Gender { get; init; }

    public required DateOnly BirthDate { get; init; }
}
