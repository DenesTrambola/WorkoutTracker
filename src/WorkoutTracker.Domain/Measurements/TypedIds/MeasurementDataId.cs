namespace WorkoutTracker.Domain.Measurements.TypedIds;

using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;

public sealed record MeasurementDataId : StronglyTypedId<Guid>
{
    private MeasurementDataId(Guid id)
        : base(id)
    {
    }

    private MeasurementDataId()
        : base(Guid.Empty)
    {
    }

    public static Result<MeasurementDataId> New() // Consider renaming to CreateNew
    {
        return new MeasurementDataId(Guid.NewGuid());
    }

    public static Result<MeasurementDataId> FromGuid(Guid value)
    {
        return Result.Ensure(
            value,
            v => v != Guid.Empty,
            DomainErrors.MeasurementDataId.Empty)
            .Map(v => new MeasurementDataId(v));
    }

    public static Result<MeasurementDataId> EnsureNotNull(MeasurementDataId id)
    {
        return Result.Ensure(
            id,
            id => id is not null,
            DomainErrors.MeasurementDataId.Null);
    }
}
