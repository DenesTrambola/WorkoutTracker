namespace WorkoutTracker.Domain.Shared.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using static WorkoutTracker.Domain.Shared.Errors.DomainErrors;

public class Description : ValueObject
{
    public const short MaxLength = 500;

    public string? Text { get; private set; }

    private Description(string? text)
    {
        Text = text;
    }

    public static Result<Description> Create(string? text)
    {
        return EnsureNotTooLong(text)
            .Map(t => new Description(t));
    }

    private static Result<string?> EnsureNotTooLong(string? text)
    {
        return Result.Ensure(
             text,
             text => text is null || MaxLength >= text.Length,
             DomainErrors.Description.TooLong);
    }

    public static Result<Description> EnsureNotNull(Description? description)
    {
        return description is not null
            ? Result.Success(description)
            : Result.Failure<Description>(DomainErrors.Description.Null);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Text;
    }
}
