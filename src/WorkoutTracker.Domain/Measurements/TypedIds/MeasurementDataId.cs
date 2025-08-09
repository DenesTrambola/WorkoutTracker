namespace WorkoutTracker.Domain.Measurements.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;

public record MeasurementDataId : StronglyTypedId<Guid>
{
    protected MeasurementDataId(Guid id)
        : base(id)
    {
    }

    public static MeasurementDataId New()
    {
        return new MeasurementDataId(Guid.NewGuid());
    }
}
