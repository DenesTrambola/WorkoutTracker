namespace WorkoutTracker.Domain.Measurement;

using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.Errors;
using WorkoutTracker.Domain.Users.TypedIds;

public class Measurement : AggregateRoot<MeasurementId>
{
    private readonly List<MeasurementData> _data = [];

    public Name Name { get; private set; }
    public Description? Description { get; private set; }
    public MeasurementUnit Unit { get; private set; }
    public UserId UserId { get; private set; }

    public User User { get; private set; }
    public IReadOnlyCollection<MeasurementData> Data => _data.AsReadOnly();

    private Measurement()
    {
        Name = null!;
        UserId = null!;
        User = null!;
    }

    private Measurement(
        MeasurementId id,
        Name name,
        Description? description,
        MeasurementUnit unit,
        User user)
        : base(id)
    {
        Name = name;
        Description = description;
        Unit = unit;
        User = user;
        UserId = user.Id;
    }

    public static Result<Measurement> Create(
        MeasurementId id,
        Name name,
        Description? description,
        MeasurementUnit unit,
        User user)
    {
        return Result.Ensure(
            user,
            u => u is not null,
            DomainErrors.User.Null)
            .Map(u => new Measurement(id, name, description, unit, u));
    }
}
