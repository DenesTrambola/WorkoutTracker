namespace WorkoutTracker.Application.Users.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    public static class AccessToken
    {
        public static Error GenerationFailed => new(
            $"{nameof(AccessToken)}.TokenGenerationFailed",
            $"Failed to generate {nameof(AccessToken)}.");
    }
}
