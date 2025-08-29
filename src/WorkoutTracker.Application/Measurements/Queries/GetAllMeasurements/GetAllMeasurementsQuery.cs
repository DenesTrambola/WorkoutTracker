namespace WorkoutTracker.Application.Measurements.Queries.GetAllMeasurements;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetAllMeasurementsQuery
    : IQuery<MeasurementListResponse>
{
}
