namespace WorkoutTracker.Domain.Measurements.TypedIds;

using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public record MeasurementDataId : StronglyTypedId<Guid>
{
    protected MeasurementDataId(Guid id)
        : base(id)
    {
    }

    public static Result<MeasurementDataId> New()
    {
        return new MeasurementDataId(Guid.NewGuid());
    }

    public static Result<MeasurementDataId> EnsureNotNull(MeasurementDataId id)
    {
        return Result.Ensure(
            id,
            id => id is not null,
            DomainErrors.MeasurementDataId.Null);
    }
}
