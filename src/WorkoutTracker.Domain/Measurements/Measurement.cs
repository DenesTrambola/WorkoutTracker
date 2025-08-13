namespace WorkoutTracker.Domain.Measurements;

using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;

public class Measurement : AggregateRoot<MeasurementId>
{
    private readonly List<MeasurementData> _data = [];

    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public MeasurementUnit Unit { get; private set; }
    public UserId UserId { get; private set; }

    public IReadOnlyCollection<MeasurementData> Data => _data.AsReadOnly();

    private Measurement()
    {
        Name = null!;
        Description = null!;
        UserId = null!;
    }

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

    public static Result<Measurement> Create(
        Name name,
        Description description,
        MeasurementUnit unit,
        UserId userId)
    {
        Result<MeasurementId> measurementIddResult = MeasurementId.New();

        return Result.Combine(
            Name.EnsureNotNull(name),
            Description.EnsureNotNull(description),
            EnsureMeasurementUnitIsDefined(unit),
            UserId.EnsureNotNull(userId))
            .OnSuccess(() => new Measurement(
                measurementIddResult.ValueOrDefault(),
                name,
                description,
                unit,
                userId));
    }

    private static Result<MeasurementUnit> EnsureMeasurementUnitIsDefined(MeasurementUnit unit)
    {
        return Result.Ensure(
            unit,
            u => Enum.IsDefined<MeasurementUnit>(u),
            DomainErrors.MeasurementUnit.Invalid);
    }

    public Result<Measurement> UpdateName(Name newName)
    {
        return Name.EnsureNotNull(newName)
            .OnSuccess(n =>
            {
                if (Name != n)
                    Name = n;
            })
            .Map(_ => this);
    }

    public Result<Measurement> UpdateDescription(Description newDescription)
    {
        return Description.EnsureNotNull(newDescription)
            .OnSuccess(d =>
            {
                if (Description != d)
                    Description = d;
            })
            .Map(_ => this);
    }

    public Result<Measurement> UpdateUnit(MeasurementUnit newUnit)
    {
        return EnsureMeasurementUnitIsDefined(newUnit)
            .OnSuccess(u =>
            {
                if (Unit != u)
                    Unit = u;
            })
            .Map(_ => this);
    }
}
