namespace WorkoutTracker.Domain.Measurements.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class MeasurementDataValue : ValueObject
{
    public float Value { get; private set; }

    private MeasurementDataValue(float value)
    {
        Value = value;
    }

    public static Result<MeasurementDataValue> Create(float value)
    {
        return EnsureIsValid(value)
            .Map(v => new MeasurementDataValue(v));
    }

    private static Result<float> EnsureIsValid(float value)
    {
        return Result.Ensure(
            value,
            v => v > 0,
            DomainErrors.MeasurementDataValue.Invalid);
    }

    public static Result<MeasurementDataValue> EnsureNotNull(MeasurementDataValue? value)
    {
        return value is not null
            ? Result.Success(value)
            : Result.Failure<MeasurementDataValue>(DomainErrors.MeasurementDataValue.Null);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
