namespace WorkoutTracker.Domain.Measurements;

using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Measurements.ValueObjects;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;

public class MeasurementData : Entity<MeasurementDataId>
{
    public MeasurementDataValue Value { get; private set; }
    public DateTime MeasuredOn { get; private set; }
    public Comment Comment { get; private set; }
    public MeasurementId MeasurementId { get; private set; }

    private MeasurementData()
    {
        Value = null!;
        Comment = null!;
        MeasurementId = null!;
    }

    private MeasurementData(
        MeasurementDataId id,
        MeasurementDataValue value,
        DateTime measuredOn,
        Comment comment,
        MeasurementId measurementId)
        : base(id)
    {
        Value = value;
        MeasuredOn = measuredOn;
        Comment = comment;
        MeasurementId = measurementId;
    }

    internal static Result<MeasurementData> Create(
        MeasurementDataValue value,
        DateTime measuredOn,
        Comment comment,
        MeasurementId measurementId)
    {
        Result<MeasurementDataId> measurementDataIdResult = MeasurementDataId.New();

        return Result.Combine(
            MeasurementDataValue.EnsureNotNull(value),
            EnsureMeasuredOnIsValid(measuredOn),
            Comment.EnsureNotNull(comment),
            MeasurementId.EnsureNotNull(measurementId),
            measurementDataIdResult)
            .OnSuccess(() => new MeasurementData(
                measurementDataIdResult.ValueOrDefault(),
                value,
                measuredOn,
                comment,
                measurementId));
    }

    private static Result<DateTime> EnsureMeasuredOnIsValid(DateTime measuredOn)
    {
        return Result.Ensure(
            measuredOn,
            mo => mo <= DateTime.UtcNow,
            DomainErrors.MeasurementData.InvalidDate);
    }

    public Result<MeasurementData> UpdateValue(MeasurementDataValue newValue)
    {
        return Result.Ensure(
            newValue,
            v => v is not null,
            DomainErrors.MeasurementDataValue.Null)
            .OnSuccess(v => Value = v)
            .Map(_ => this);
    }

    public Result<MeasurementData> UpdateMeasuredOn(DateTime newMeasuredOn)
    {
        return Result.Ensure(
            newMeasuredOn,
            mo => mo <= DateTime.UtcNow,
            DomainErrors.MeasurementData.InvalidDate)
            .OnSuccess(mo => MeasuredOn = mo)
            .Map(_ => this);
    }

    public Result<MeasurementData> UpdateComment(Comment newComment)
    {
        return Result.Ensure(
            newComment,
            c => c is not null,
            Shared.Errors.DomainErrors.Comment.Null)
            .OnSuccess(c => Comment = c)
            .Map(_ => this);
    }
}
