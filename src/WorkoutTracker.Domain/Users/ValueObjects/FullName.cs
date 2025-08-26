namespace WorkoutTracker.Domain.Users.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public class FullName : ValueObject
{
    public const byte MaxLength = 64;

    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<FullName> Create(string firstName, string lastName)
    {
        return Result.Combine(
            EnsureFirstNameNotEmpty(firstName),
            EnsureLastNameNotEmpty(lastName),
            EnsureFirstNameNotTooLong(firstName),
            EnsureLastNameNotTooLong(lastName))
            .OnSuccess(() => new FullName(firstName, lastName));
    }

    private static Result<string> EnsureFirstNameNotEmpty(string firstName)
    {
        return Result.Ensure(
            firstName,
            fn => !string.IsNullOrWhiteSpace(fn),
            DomainErrors.FirstName.Empty);
    }

    private static Result<string> EnsureLastNameNotEmpty(string lastName)
    {
        return Result.Ensure(
            lastName,
            ln => !string.IsNullOrWhiteSpace(ln),
            DomainErrors.LastName.Empty);
    }

    private static Result<string> EnsureFirstNameNotTooLong(string firstName)
    {
        return Result.Ensure(
            firstName,
            fn => fn.Length <= MaxLength,
            DomainErrors.FirstName.TooLong);
    }

    private static Result<string> EnsureLastNameNotTooLong(string lastName)
    {
        return Result.Ensure(
            lastName,
            ln => ln.Length <= MaxLength,
            DomainErrors.LastName.TooLong);
    }

    public static Result<FullName> EnsureNotNull(FullName? fullName)
    {
        return fullName is not null
            ? Result.Success(fullName)
            : Result.Failure<FullName>(DomainErrors.FullName.Null);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return FirstName;
        yield return LastName;
    }
}
