namespace WorkoutTracker.Domain.Users.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public class PasswordHash : ValueObject
{
    public string Value { get; private set; }

    private PasswordHash(string passwordHash)
        => Value = passwordHash;

    public static Result<PasswordHash> Create(string passwordHash)
        => Result.Ensure(
            passwordHash,
            ph => !string.IsNullOrWhiteSpace(ph),
            DomainErrors.PasswordHash.Empty)
        .Map(ph => new PasswordHash(ph));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
