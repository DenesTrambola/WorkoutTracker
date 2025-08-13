namespace WorkoutTracker.Domain.Users.ValueObjects;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public class Email : ValueObject
{
    public const short MaxLength = 320;

    public string Address { get; private set; }

    private Email(string address)
    {
        Address = address;
    }

    public static Result<Email> Create(string address)
    {
        return Result.Combine(
            EnsureNotEmpty(address),
            EnsureNotTooLong(address),
            EnsureFormatIsValid(address))
        .Map(a => new Email(a));
    }

    private static Result<string> EnsureNotEmpty(string address)
    {
        return Result.Ensure(
            address,
            address => !string.IsNullOrWhiteSpace(address),
            DomainErrors.Email.Empty);
    }

    private static Result<string> EnsureNotTooLong(string address)
    {
        return Result.Ensure(
            address,
            address => MaxLength >= address.Length,
            DomainErrors.Email.TooLong);
    }

    private static Result<string> EnsureFormatIsValid(string address)
    {
        return Result.Ensure(
            address,
            address => Regex.IsMatch(address, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"),
            DomainErrors.Email.InvalidFormat);
    }

    public static Result<Email> EnsureNotNull(Email? email)
    {
        return email is not null
            ? Result.Success(email)
            : Result.Failure<Email>(DomainErrors.Email.Null);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Address;
    }
}
