namespace WorkoutTracker.Domain.Measurements.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public class MeasurementDataValue : ValueObject
{
    public short Value { get; private set; }

    private MeasurementDataValue(short value)
        => Value = value;

    public static Result<MeasurementDataValue> Create(short value)
        => new MeasurementDataValue(value);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
