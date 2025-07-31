namespace WorkoutTracker.Domain.Shared.Results;

using WorkoutTracker.Domain.Shared.Errors;

public class Result<TValue> : Result
{
    private readonly TValue _value;

    public TValue Value => IsSuccess
        ? _value : throw new InvalidOperationException("Cannot access value of a failed result.");

    protected internal Result(TValue value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public static implicit operator Result<TValue>(TValue value)
        => new Result<TValue>(value, true, Error.None);

    public Result<TValue> ToResult(TValue value) => value;
}
