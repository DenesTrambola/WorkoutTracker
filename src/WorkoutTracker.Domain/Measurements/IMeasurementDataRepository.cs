namespace WorkoutTracker.Domain.Measurements;

using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;

public interface IMeasurementDataRepository
    : IRepository<MeasurementData, MeasurementDataId>
{
}
