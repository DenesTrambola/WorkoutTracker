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

    public static Result<TValue> OnSuccess<TValue>(
        [NotNull] this Result<TValue> result,
        [NotNull] Action<TValue> action)
    {
        if (result.IsSuccess)
            action(result.ValueOrDefault());

        return result;
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
        [NotNull] this Result<TOut> result,
        [NotNull] Func<TOut> mapFunc)
    {
        if (result.IsFailure)
            return Result.Failure<TOut>(result.Errors);

        return Result.Success<TOut>(mapFunc());
    }
}
