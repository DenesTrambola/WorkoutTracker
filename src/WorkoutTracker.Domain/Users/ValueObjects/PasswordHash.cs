namespace WorkoutTracker.Domain.Users.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public class PasswordHash : ValueObject
{
    public string Value { get; private set; }

    private PasswordHash(string passwordHash)
    {
        Value = passwordHash;
    }

    public static Result<PasswordHash> Create(string passwordHash)
    {
        return EnsureNotEmpty(passwordHash)
            .Map(ph => new PasswordHash(ph));
    }

    private static Result<string> EnsureNotEmpty(string passwordHash)
    {
        return Result.Ensure(
            passwordHash,
            ph => !string.IsNullOrWhiteSpace(ph),
            DomainErrors.PasswordHash.Empty);
    }

    public static Result<PasswordHash> EnsureNotNull(PasswordHash? passwordHash)
    {
        return passwordHash is not null
            ? Result.Success(passwordHash)
            : Result.Failure<PasswordHash>(DomainErrors.PasswordHash.Null);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
