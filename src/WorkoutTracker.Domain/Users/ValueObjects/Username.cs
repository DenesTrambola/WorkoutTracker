namespace WorkoutTracker.Domain.Users.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Errors;

public class Username : ValueObject
{
    public const short MaxLength = 32;

    public string Login { get; private set; }

    private Username(string login)
        => Login = login;

    public static Result<Username> Create(string login)
        => Result.Combine(
            Result.Ensure(
                login,
                login => !string.IsNullOrWhiteSpace(login),
                DomainErrors.Username.Empty),
            Result.Ensure(
                login,
                login => login.Length <= MaxLength,
                DomainErrors.Username.TooLong))
        .Map(l => new Username(l));

    private static Result<string> EmptyCheck(string login)
        => Result.Ensure(
            login,
            login => !string.IsNullOrWhiteSpace(login),
            DomainErrors.Username.Empty);

    private static Result<string> LengthCheck(string login)
        => Result.Ensure(
            login,
            login => login.Length <= MaxLength,
            DomainErrors.Username.TooLong);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Login;
    }
}
