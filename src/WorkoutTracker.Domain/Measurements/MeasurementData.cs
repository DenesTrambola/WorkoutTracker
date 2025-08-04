namespace WorkoutTracker.Domain.Measurement;

using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Measurements.ValueObjects;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;

public class MeasurementData : Entity<MeasurementDataId>
{
    public MeasurementDataValue Value { get; private set; }
    public DateOnly Date { get; private set; }
    public Comment? Comment { get; private set; }
    public MeasurementId MeasurementId { get; private set; }

    public Measurement Measurement { get; private set; }

    private MeasurementData()
    {
        Value = null!;
        MeasurementId = null!;
        Measurement = null!;
    }

    private MeasurementData(
        MeasurementDataId id,
        MeasurementDataValue value,
        Comment? comment,
        Measurement measurement)
        : base(id)
    {
        Value = value;
        Comment = comment;
        Measurement = measurement;
        MeasurementId = measurement.Id;
    }

    internal static Result<MeasurementData> Create(
        MeasurementDataId id,
        MeasurementDataValue value,
        Comment? comment,
        Measurement measurement)
    {
        return new MeasurementData(id, value, comment, measurement);
    }
}
