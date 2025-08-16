namespace WorkoutTracker.Domain.Shared.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Comment : ValueObject
{
    public const short MaxLength = 500;

    public string? Text { get; private set; }

    private Comment(string? text)
    {
        Text = text;
    }

    public static Result<Comment> Create(string? text)
    {
        return EnsureNotTooLong(text)
            .Map(t => new Comment(t));
    }

    private static Result<string?> EnsureNotTooLong(string? text)
    {
        return Result.Ensure(
            text,
            text => text is null || MaxLength >= text.Length,
            DomainErrors.Comment.TooLong);
    }

    public static Result<Comment> EnsureNotNull(Comment? comment)
    {
        return comment is not null
            ? Result.Success(comment)
            : Result.Failure<Comment>(DomainErrors.Comment.Null);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Text;
    }
}
