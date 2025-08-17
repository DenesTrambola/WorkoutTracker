namespace WorkoutTracker.Application.Shared.Primitives;

public interface IAccessTokenProvider
{
    string GenerateToken(Guid userId, string email, string role);
}
