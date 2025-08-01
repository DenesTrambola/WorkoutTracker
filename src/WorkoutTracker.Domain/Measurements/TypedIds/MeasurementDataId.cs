namespace WorkoutTracker.Domain.Measurements.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;

public record MeasurementDataId(Guid Value) : StronglyTypedId<Guid>(Value);
