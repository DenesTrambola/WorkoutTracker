namespace WorkoutTracker.Domain.Measurements.TypedIds;

using WorkoutTracker.Domain.Shared.Primitives;

public record MeasurementId(Guid Value) : StronglyTypedId<Guid>(Value);
