namespace WorkoutTracker.Domain.Measurements.ValueObjects;

using System.Collections.Generic;
using WorkoutTracker.Domain.Shared.Primitives;

public class MeasurementDataValue : ValueObject
{
    public int Value { get; private set; }

    private MeasurementDataValue(int value)
    {
        Value = value;
    }

    public static MeasurementDataValue Create(int value)
    {
        return new MeasurementDataValue(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
