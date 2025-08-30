namespace WorkoutTracker.Domain.Measurements;

using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public interface IMeasurementRepository
    : IRepository<Measurement, MeasurementId>
{
    Task<Result<IEnumerable<Measurement>>> GetAllByUserAsync(
        UserId userId,
        CancellationToken cancellationToken = default);

    Task<Result<Name>> ValidateNameUniqueness(
        Name name,
        UserId userId,
        CancellationToken cancellationToken = default);
}
