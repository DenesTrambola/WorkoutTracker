namespace WorkoutTracker.Domain.Shared.Exceptions;

public class ResultInvariantViolationException(string message) : InvalidOperationException(message)
{
}
