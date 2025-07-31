namespace WorkoutTracker.Domain.Shared.Exceptions;

public class NullErrorException : Exception
{
    public NullErrorException()
        : base("Error cannot be null.")
    {
    }
}
