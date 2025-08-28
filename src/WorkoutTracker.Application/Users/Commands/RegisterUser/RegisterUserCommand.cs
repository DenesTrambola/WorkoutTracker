namespace WorkoutTracker.Application.Users.Commands.RegisterUser;

using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed record RegisterUserCommand
    : ICommand
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public byte Gender { get; init; }
    public DateOnly BirthDate { get; init; }
}
