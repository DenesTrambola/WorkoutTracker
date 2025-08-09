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
        return Result.Zip(
            EnsureNotEmpty(firstName, lastName),
            EnsureNotTooLong(firstName, lastName),
            (first, last) => new FullName(first, last));
    }

    private static Result<string> EnsureNotEmpty(string firstName, string lastName)
    {
        return Result.Combine(
            Result.Ensure(
                firstName,
                firstName => !string.IsNullOrWhiteSpace(firstName),
                DomainErrors.FirstName.Empty),
            Result.Ensure(
                lastName,
                lastName => !string.IsNullOrWhiteSpace(lastName),
                DomainErrors.LastName.Empty));
    }

    private static Result<string> EnsureNotTooLong(string firstName, string lastName)
    {
        return Result.Ensure(
            firstName,
            firstName => firstName.Length <= MaxLength,
            DomainErrors.FirstName.TooLong);
    }

    public static Result<FullName> EnsureNotNull(FullName fullName)
    {
        return Result.Ensure(
            fullName,
            fn => fn is not null,
            DomainErrors.FullName.Null);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return FirstName;
        yield return LastName;
    }
}
