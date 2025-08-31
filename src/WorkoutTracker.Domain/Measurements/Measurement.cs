namespace WorkoutTracker.Domain.Measurements;

using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Measurements.ValueObjects;
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
            u => Enum.IsDefined(u),
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

    public Result<Measurement> ReassignToUser(UserId newUserId)
    {
        return UserId.EnsureNotNull(newUserId)
            .OnSuccess(uId =>
            {
                if (UserId != uId)
                    UserId = uId;
            })
            .Map(_ => this);
    }

    public Result<MeasurementData> AddData(
        MeasurementDataValue value,
        DateTime measuredOn,
        Comment comment)
    {
        return Result.Combine(
            MeasurementDataValue.EnsureNotNull(value),
            Comment.EnsureNotNull(comment))
            .OnSuccess(() => MeasurementData.Create(value, measuredOn, comment, Id))
            .OnSuccess(d => _data.Add(d));
    }

    public Result<Measurement> RemoveData(MeasurementDataId dataId)
    {
        return MeasurementDataId.EnsureNotNull(dataId)
            .Map(dId => _data.Find(data => data.Id == dId))
            .Ensure(d => d is not null, DomainErrors.MeasurementData.NotFound)
            .Ensure(d => _data.Remove(d!), DomainErrors.MeasurementData.CannotRemove)
            .Map(_ => this);
    }
}
