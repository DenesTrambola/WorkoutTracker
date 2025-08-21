namespace WorkoutTracker.Domain.Shared.Results;

using System.Diagnostics.CodeAnalysis;
using WorkoutTracker.Domain.Shared.Errors;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error[] Errors { get; }

    protected internal Result(bool isSuccess, [NotNull] params Error[] errors)
    {
        if (isSuccess && errors.Length > 0)
            errors = new[] { DomainErrors.None };
        
        if (!isSuccess && errors.Length == 0)
            errors = new[] { DomainErrors.Unknown };
        
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success()
    {
        return new Result(true, DomainErrors.None);
    }

    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(value, true, DomainErrors.None);
    }

    public static Result Failure(params Error[] errors)
    {
        var newErrors = errors
            ?.Where(e => e != DomainErrors.None).ToArray()
            ?? new[] { DomainErrors.Unknown };

        return new Result(false, newErrors);
    }

    public static Result<TValue> Failure<TValue>(params Error[] errors)
    {
        var newErrors = errors
            ?.Where(e => e != DomainErrors.None).ToArray()
            ?? new[] { DomainErrors.Unknown };

        return new Result<TValue>(default!, false, newErrors);
    }

    public static Result Ensure(bool condition, [NotNull] Error error)
        => condition ? Success() : Failure(error);

    public static Result<TValue> Ensure<TValue>(
        TValue value,
        [NotNull] Func<TValue, bool> predicate,
        Error error)
    {
        return predicate(value) ? Success(value) : Failure<TValue>(error);
    }

    public static Result Combine(params Result[] results)
    {
        if (results == null || results.Length == 0)
            return Failure();

        if (results.Any(r => r.IsFailure))
            return Failure(results.SelectMany(r => r.Errors).Distinct().ToArray());

        return Success();
    }

    public static Result<TValue> Combine<TValue>(params Result<TValue>[] results)
    {
        if (results == null || results.Length == 0)
            return Failure<TValue>();

        if (results.Any(r => r.IsFailure))
            return Failure<TValue>(results.SelectMany(r => r.Errors).Distinct().ToArray());

        return Success(results[0].ValueOrDefault());
    }

    public static Result<TOut> Zip<TIn1, TIn2, TOut>(
        [NotNull] Result<TIn1> first,
        [NotNull] Result<TIn2> second,
        [NotNull] Func<TIn1, TIn2, Result<TOut>> map)
    {
        if (first.IsFailure || second.IsFailure)
        {
            var allErrors = first.Errors.Concat(second.Errors).Distinct().ToArray();
            return Result.Failure<TOut>(allErrors);
        }

        return map(first.ValueOrDefault(), second.ValueOrDefault());
    }

    public static Result<TOut> Zip<TIn1, TIn2, TOut>(
        [NotNull] Result<TIn1> first,
        [NotNull] Result<TIn2> second,
        [NotNull] Func<TIn1, TIn2, TOut> map)
    {
        if (first.IsFailure || second.IsFailure)
        {
            var allErrors = first.Errors.Concat(second.Errors).Distinct().ToArray();
            return Result.Failure<TOut>(allErrors);
        }

        return Result.Success(map(first.ValueOrDefault(), second.ValueOrDefault()));
    }
}
