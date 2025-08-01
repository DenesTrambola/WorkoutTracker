namespace WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class NameErrors
    {
        public static readonly Error NullOrWhiteSpace = new Error(
            "Name.NullOrWhiteSpace",
            "The name cannot be null or white space.");
    }
}
