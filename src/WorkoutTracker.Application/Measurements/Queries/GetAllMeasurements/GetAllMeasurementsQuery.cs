namespace WorkoutTracker.Application.Measurements.Queries.GetAllMeasurements;

using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements.Enums;

public sealed record GetAllMeasurementsQuery
    : IQuery<IEnumerable<MeasurementResponse>>
{
    public string? Name { get; init; }

    public string? Description { get; init; }

    public MeasurementUnit? Unit { get; init; }

    public Guid? UserId { get; init; }
}
