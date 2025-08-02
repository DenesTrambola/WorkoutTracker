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
        => Result.Zip(
            FullNameEmptyCheck(firstName, lastName),
            FullNameLengthCheck(firstName, lastName),
            (first, last) => new FullName(first, last));

    private static Result<string> FullNameEmptyCheck(string firstName, string lastName)
        => Result.Combine(
            Result.Ensure(
                firstName,
                firstName => !string.IsNullOrWhiteSpace(firstName),
                DomainErrors.FirstName.Empty),
            Result.Ensure(
                lastName,
                lastName => !string.IsNullOrWhiteSpace(lastName),
                DomainErrors.LastName.Empty));

    private static Result<string> FullNameLengthCheck(string firstName, string lastName)
        => Result.Ensure(
            firstName,
            firstName => firstName.Length <= MaxLength,
            DomainErrors.FirstName.TooLong);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return FirstName;
        yield return LastName;
    }
}
