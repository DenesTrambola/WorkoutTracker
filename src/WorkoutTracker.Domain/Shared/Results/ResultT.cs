namespace WorkoutTracker.Domain.Shared.Results;

using WorkoutTracker.Domain.Shared.Errors;

public class Result<TValue> : Result
{
    private readonly TValue _value;

    protected internal Result(TValue value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    protected internal Result(TValue value, bool isSuccess, Error[] errors)
        : base(isSuccess, errors)
    {
        _value = value;
    }

    public TValue ValueOrDefault() => IsSuccess ? _value : default!;

    public TValue ValueOr(TValue defaultValue)
        => IsSuccess ? _value : defaultValue;

    public static implicit operator Result<TValue>(TValue value)
        => new Result<TValue>(value, true, Error.None);

    public Result<TValue> ToResult(TValue value) => value;
}
