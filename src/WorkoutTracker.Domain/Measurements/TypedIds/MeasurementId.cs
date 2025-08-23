namespace WorkoutTracker.Domain.Measurements.TypedIds;

using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed record MeasurementId : StronglyTypedId<Guid>
{
    private MeasurementId(Guid id)
        : base(id)
    {
    }

    private MeasurementId()
        : base(Guid.Empty)
    {
    }

    public static Result<MeasurementId> New() // Consider renaming to CreateNew
    {
        return new MeasurementId(Guid.NewGuid());
    }

    public static Result<MeasurementId> FromGuid(Guid value)
    {
        return Result.Ensure(
            value,
            v => v != Guid.Empty,
            DomainErrors.MeasurementId.Empty)
            .Map(v => new MeasurementId(v));
    }

    public static Result<MeasurementId> EnsureNotNull(MeasurementId measurementId)
    {
        return Result.Ensure(
            measurementId,
            mId => mId is not null,
            DomainErrors.MeasurementId.Null);
    }
}
