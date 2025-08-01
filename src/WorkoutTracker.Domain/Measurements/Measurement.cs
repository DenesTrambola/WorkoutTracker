namespace WorkoutTracker.Domain.Measurement;

using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public class Measurement : AggregateRoot<MeasurementId>
{
    private readonly List<MeasurementData> _data = new();

    public Name Name { get; private set; }
    public Description? Description { get; private set; }
    public MeasurementUnit Unit { get; private set; }
    public UserId UserId { get; private set; }
    public IReadOnlyCollection<MeasurementData> Data => _data.AsReadOnly();

    private Measurement(
        MeasurementId id,
        Name name,
        Description description,
        MeasurementUnit unit,
        UserId userId)
        : base(id)
    {
        Name = name;
        Description = description;
        Unit = unit;
        UserId = userId;
    }

    public static Measurement Create(
        MeasurementId id,
        Name name,
        Description description,
        MeasurementUnit unit,
        UserId userId)
    {
        return new Measurement(id, name, description, unit, userId);
    }
}
