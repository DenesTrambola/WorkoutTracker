namespace WorkoutTracker.Domain.Shared.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class Description : ValueObject
{
    public const short MaxLength = 500;

    public string Text { get; private set; }

    private Description(string text)
        => Text = text;

    public static Result<Description?> Create(string? text)
        => LengthCheck(text)
        .Map(t => t is null ? null : new Description(t));

    private static Result<string?> LengthCheck(string? text)
        => Result.Ensure(
            text,
            text => MaxLength > text?.Length,
            DomainErrors.Description.TooLong);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Text;
    }
}
