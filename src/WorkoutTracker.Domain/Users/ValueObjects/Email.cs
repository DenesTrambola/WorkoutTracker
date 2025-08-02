namespace WorkoutTracker.Domain.Users.ValueObjects;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public class Email : ValueObject
{
    public const short MaxLength = 320;

    public string Address { get; }

    private Email(string address)
        => Address = address;

    public static Result<Email> Create(string address)
        => Result.Combine(
            EmptyCheck(address),
            LengthCheck(address),
            FormatCheck(address))
        .Map(a => new Email(a));

    private static Result<string> EmptyCheck(string address)
        => Result.Ensure(
            address,
            address => !string.IsNullOrWhiteSpace(address),
            DomainErrors.Email.Empty);

    private static Result<string> LengthCheck(string address)
        => Result.Ensure(
            address,
            address => MaxLength > address.Length,
            DomainErrors.Email.TooLong);

    private static Result<string> FormatCheck(string address)
        => Result.Ensure(
            address,
            address => Regex.IsMatch(address, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"),
            DomainErrors.Email.InvalidFormat);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Address;
    }
}
