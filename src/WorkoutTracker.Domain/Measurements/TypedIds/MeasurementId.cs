namespace WorkoutTracker.Domain.Measurements.TypedIds;

using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public record MeasurementId : StronglyTypedId<Guid>
{
    protected MeasurementId(Guid id)
        : base(id)
    {
    }

    public static MeasurementId New()
    {
        return new MeasurementId(Guid.NewGuid());
    }

    public static Result<MeasurementId> EnsureNotNull(MeasurementId measurementId)
    {
        return Result.Ensure(
            measurementId,
            mId => mId is not null,
            DomainErrors.MeasurementId.Null);
    }
}
