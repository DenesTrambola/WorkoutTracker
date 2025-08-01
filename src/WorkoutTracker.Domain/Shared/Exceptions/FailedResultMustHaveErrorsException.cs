namespace WorkoutTracker.Domain.Shared.Exceptions;

public class FailedResultMustHaveErrorsException : ResultInvariantViolationException
{
    public FailedResultMustHaveErrorsException()
        : base("Failed result must have an error.")
    {
    }
}
