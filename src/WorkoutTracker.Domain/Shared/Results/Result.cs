namespace WorkoutTracker.Domain.Shared.Results;

using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Exceptions;

public class Result : IEquatable<Result>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Successful result cannot have an error.");
        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Failed result must have an error.");

        IsSuccess = isSuccess;
        Error = error ?? throw new NullErrorException();
    }

    public static Result Success()
        => new Result(true, Error.None);

    public static Result Failure(Error error)
        => new Result(false, error ?? throw new NullErrorException());

    public bool Equals(Result? other)
        => other is not null && IsSuccess == other.IsSuccess && Error == other.Error;

    public override bool Equals(object? obj)
        => Equals(obj as Result);

    public override int GetHashCode()
        => HashCode.Combine(IsSuccess, Error);

    public static bool operator ==(Result? left, Result? right)
        => left is not null && left.Equals(right);

    public static bool operator !=(Result? left, Result? right)
        => !(left == right);
}
