namespace WorkoutTracker.Application.Users.Primitives;

using WorkoutTracker.Application.Users.Models;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public interface IAccessTokenProvider
{
    Result<AccessToken> GenerateToken(UserId userId, Email email, UserRole role);
}
