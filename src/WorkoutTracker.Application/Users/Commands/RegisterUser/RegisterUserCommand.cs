namespace WorkoutTracker.Application.Users.Commands.RegisterUser;

using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed record RegisterUserCommand(
    string Username,
    string Password,
    string Email,
    string FirstName,
    string LastName,
    byte Gender,
    DateOnly BirthDate)
    : ICommand;
