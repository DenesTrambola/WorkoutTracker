namespace WorkoutTracker.Domain.Shared.Exceptions;

public class NullErrorException : InvalidOperationException
{
    public NullErrorException()
        : base("Error cannot be null.")
    {
    }
}
