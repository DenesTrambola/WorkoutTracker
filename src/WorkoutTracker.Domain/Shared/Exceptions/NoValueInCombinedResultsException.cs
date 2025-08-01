namespace WorkoutTracker.Domain.Shared.Exceptions;

public class NoValueInCombinedResultsException : InvalidOperationException
{
    public NoValueInCombinedResultsException()
        : base("Cannot combine results because no non-null value was found.")
    {
    }
}
