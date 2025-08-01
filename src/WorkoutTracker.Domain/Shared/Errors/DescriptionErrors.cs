namespace WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class DescriptionErrors
    {
        public static Error NullOrWhiteSpace => new(
            "Description.NullOrWhiteSpace",
            "The description cannot be null or white space."
        );
    }
}
