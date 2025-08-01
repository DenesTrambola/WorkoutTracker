namespace WorkoutTracker.Domain.Shared.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class Comment : ValueObject
{
    public string Text { get; private set; }

    private Comment(string text)
    {
        Text = text;
    }

    public static Result<Comment?> Create(string text)
    {
        return new Comment(text);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Text;
    }
}
