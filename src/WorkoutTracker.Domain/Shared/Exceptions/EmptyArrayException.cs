namespace WorkoutTracker.Domain.Shared.Exceptions;

public class EmptyArrayException : InvalidOperationException
{
    public EmptyArrayException()
        : base("Array cannot be empty.")
    {
    }
}
