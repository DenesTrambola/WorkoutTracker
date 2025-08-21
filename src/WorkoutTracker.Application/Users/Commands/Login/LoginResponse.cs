namespace WorkoutTracker.Application.Users.Commands.Login;

public sealed record LoginResponse(
    Guid UserId,
    string Username,
    string Email,
    string Token,
    DateTime ExpiredAt);
