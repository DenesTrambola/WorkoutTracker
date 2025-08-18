namespace WorkoutTracker.Application.Users.Primitives;

using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public interface IAccessTokenProvider
{
    string GenerateToken(UserId userId, Email email, UserRole role);
}
