namespace WorkoutTracker.Domain.Shared.Exceptions;

public class SuccessfulResultCannotHaveErrorsException : ResultInvariantViolationException
{
    public SuccessfulResultCannotHaveErrorsException()
        : base("Successful result cannot have errors.")
    {
    }
}
