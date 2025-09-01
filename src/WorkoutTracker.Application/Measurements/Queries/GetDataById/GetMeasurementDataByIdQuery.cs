namespace WorkoutTracker.Application.Measurements.Queries.GetDataById;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record GetMeasurementDataByIdQuery(Guid Id)
    : IQuery<MeasurementDataResponse>;
