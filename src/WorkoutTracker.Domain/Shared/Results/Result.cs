namespace WorkoutTracker.Domain.Shared.Results;

using System.Diagnostics.CodeAnalysis;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Exceptions;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error[] Errors { get; }

    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new SuccessfulResultCannotHaveErrorsException();

        if (!isSuccess && error == Error.None)
            throw new FailedResultMustHaveErrorsException();

        IsSuccess = isSuccess;
        Errors = new[] { error };
    }

    protected internal Result(bool isSuccess, [NotNull] Error[] errors)
    {
        if (isSuccess && errors.Length > 0)
            throw new SuccessfulResultCannotHaveErrorsException();

        if (!isSuccess && errors.Length == 0)
            throw new FailedResultMustHaveErrorsException();

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success()
        => new Result(true, Error.None);

    public static Result Failure(Error error)
        => new Result(false, error ?? throw new NullErrorException());

    public static Result Failure(params Error[] errors)
        => new Result(false, errors ?? throw new EmptyArrayException());

    public static Result<TValue> Success<TValue>(TValue value)
        => new Result<TValue>(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error)
        => new Result<TValue>(default!, false, error ?? throw new NullErrorException());

    public static Result<TValue> Failure<TValue>(params Error[] errors)
        => new Result<TValue>(default!, false, errors ?? throw new EmptyArrayException());

    public static Result<TValue> Ensure<TValue>(
        TValue value,
        [NotNull] Func<TValue, bool> predicate, Error error)
    {
        return predicate(value)
            ? Success(value)
            : Failure<TValue>(error);
    }

    public static Result<TValue> Combine<TValue>(params Result<TValue>[] results)
    {
        if (results == null || results.Length == 0)
            throw new NoValueInCombinedResultsException();

        if (results.Any(r => r.IsFailure))
            return Failure<TValue>(results.SelectMany(r => r.Errors).Distinct().ToArray());

        return Success(results[0].ValueOrDefault());
    }
}
