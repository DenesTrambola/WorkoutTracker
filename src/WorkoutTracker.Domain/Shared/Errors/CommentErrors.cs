namespace WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Comment
    {
        public static readonly Error TooLong = TooLong(
            nameof(Comment),
            ValueObjects.Comment.MaxLength);

        public static readonly Error Null = Null(
            nameof(Comment));
    }
}
