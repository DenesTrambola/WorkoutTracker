namespace WorkoutTracker.Domain.Measurement;

using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Measurements.ValueObjects;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.ValueObjects;

public class MeasurementData : Entity<MeasurementDataId>
{
    public MeasurementDataValue Value { get; private set; }
    public DateOnly Date { get; private set; }
    public Comment? Comment { get; private set; }
    public MeasurementId MeasurementId { get; private set; }

    private MeasurementData(
        MeasurementDataId id,
        MeasurementDataValue value,
        Comment? comment,
        MeasurementId measurementId)
        : base(id)
    {
        Value = value;
        Comment = comment;
        MeasurementId = measurementId;
    }

    internal static MeasurementData Create(
        MeasurementDataId id,
        MeasurementDataValue value,
        Comment? comment,
        MeasurementId measurementId)
    {
        return new MeasurementData(id, value, comment, measurementId);
    }
}
