namespace WorkoutTracker.Domain.Shared.Results;

using System.Diagnostics.CodeAnalysis;
using WorkoutTracker.Domain.Shared.Errors;

public static class ResultExtensions
{
    public static Result<TValue> Ensure<TValue>(
        [NotNull] this Result<TValue> result,
        [NotNull] Func<TValue, bool> predicate,
        Error error)
    {
        if (result.IsFailure)
            return result;

        return predicate(result.ValueOrDefault())
            ? result
            : Result.Failure<TValue>(error);
    }

    public async static Task<Result<TValue>> EnsureAsync<TValue>(
        [NotNull] this Result<TValue> result,
        [NotNull] Func<TValue, Task<bool>> predicate,
        Error error)
    {
        if (result.IsFailure)
            return result;

        return await predicate(result.ValueOrDefault())
            ? result
            : Result.Failure<TValue>(error);
    }

    public async static Task<Result<TValue>> EnsureAsync<TValue>(
        [NotNull] this Result<TValue> result,
        [NotNull] Func<TValue, Task<Result<TValue>>> predicate,
        Error error)
    {
        return result.IsSuccess
            ? await predicate(result.ValueOrDefault())
            : result;
    }

    public static Result<TValue> OnSuccess<TValue>(
        [NotNull] this Result result,
        [NotNull] Func<TValue> callback)
    {
        if (result.IsSuccess)
        {
            var value = callback();
            return Result.Success(value);
        }

        return Result.Failure<TValue>(result.Errors);
    }

    public static Result<TValue> OnSuccess<TValue>(
        [NotNull] this Result<TValue> result,
        [NotNull] Func<Result<TValue>> callback)
    {
        if (result.IsFailure)
            return Result.Failure<TValue>(result.Errors);

        return callback();
    }

    public static Result<TValue> OnSuccess<TValue>(
        [NotNull] this Result result,
        [NotNull] Func<Result<TValue>> callback)
    {
        return result.IsSuccess
            ? callback()
            : Result.Failure<TValue>(result.Errors);
    }

    public static Result<TValue> OnSuccess<TValue>(
        [NotNull] this Result<TValue> result,
        [NotNull] Action<TValue> action)
    {
        if (result.IsSuccess)
            action(result.ValueOrDefault());

        return result;
    }

    public static Result<TValue> OnSuccess<TValue>(
        [NotNull] this Result<TValue> result,
        [NotNull] Func<TValue, Result<TValue>> action)
    {
        return result.IsSuccess
            ? action(result.ValueOrDefault())
            : Result.Failure<TValue>(result.Errors);
    }

    public async static Task<Result> OnSuccessAsync<TValue>(
        [NotNull] this Result<TValue> result,
        [NotNull] Func<TValue, Task<Result>> callback)
    {
        return result.IsSuccess
            ? await callback(result.ValueOrDefault())
            : Result.Failure(result.Errors);
    }

    public static async Task<Result> OnSuccessAsync(
        [NotNull] this Result result,
        [NotNull] Func<Task<Result>> callback)
    {
        return result.IsSuccess
            ? await callback()
            : Result.Failure(result.Errors);
    }

    public static async Task<Result<TValue>> OnSuccessAsync<TValue>(
        [NotNull] this Result result,
        [NotNull] Func<Task<Result<TValue>>> callback)
    {
        return result.IsSuccess
            ? await callback()
            : Result.Failure<TValue>(result.Errors);
    }

    public static async Task<Result<TValue>> OnSuccessAsync<TValue>(
        [NotNull] this Result<TValue> result,
        [NotNull] Func<TValue, Task<Result<TValue>>> callback)
    {
        return result.IsSuccess
            ? await callback(result.ValueOrDefault())
            : result;
    }

    public static Result<TValue> OnFailure<TValue>(
        [NotNull] this Result<TValue> result,
        [NotNull] Action<Error[]> action)
    {
        if (result.IsFailure)
            action(result.Errors);

        return result;
    }

    public static void Match<TValue>(
        [NotNull] this Result<TValue> result,
        [NotNull] Action<TValue> onSuccess,
        [NotNull] Action<Error[]> onFailure)
    {
        if (result.IsFailure)
        {
            onFailure(result.Errors);
            return;
        }

        onSuccess(result.ValueOrDefault());
    }

    public static Result<TOut> Map<TIn, TOut>(
        [NotNull] this Result<TIn> result,
        [NotNull] Func<TIn, TOut> mapFunc)
    {
        return result.IsSuccess
            ? Result.Success(mapFunc(result.ValueOrDefault()))
            : Result.Failure<TOut>(result.Errors);
    }

    public async static Task<Result<TOut>> MapAsync<TIn, TOut>(
        [NotNull] this Result<TIn> result,
        [NotNull] Func<TIn, Task<TOut>> mapFunc)
    {
        return result.IsSuccess
            ? Result.Success(await mapFunc(result.ValueOrDefault()))
            : Result.Failure<TOut>(result.Errors);
    }

    public static Result<TOut> Map<TIn, TOut>(
        [NotNull] this Result<TIn> result,
        [NotNull] Func<TIn, Result<TOut>> mapFunc)
    {
        return result.IsSuccess
            ? mapFunc(result.ValueOrDefault())
            : Result.Failure<TOut>(result.Errors);
    }

    public async static Task<Result<TOut>> MapAsync<TIn, TOut>(
        [NotNull] this Result<TIn> result,
        [NotNull] Func<TIn, Task<Result<TOut>>> mapFunc)
    {
        return result.IsSuccess
            ? await mapFunc(result.ValueOrDefault())
            : Result.Failure<TOut>(result.Errors);
    }
}
