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
        return Result.Ensure(
            value,
            v => v >= 1,
            DomainErrors.MeasurementDataValue.Null)
            .Map(v => new MeasurementDataValue(v));
    }

    public static Result<MeasurementDataValue> EnsureNotNull(MeasurementDataValue value)
    {
        return Result.Ensure(
            value,
            v => v is not null,
            DomainErrors.MeasurementDataValue.Null);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
