namespace WorkoutTracker.Application.Measurements.Queries.GetById;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetMeasurementByIdQuery(Guid Id)
    : IQuery<MeasurementResponse>;
