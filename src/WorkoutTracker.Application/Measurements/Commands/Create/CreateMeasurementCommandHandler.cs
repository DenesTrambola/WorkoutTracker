namespace WorkoutTracker.Application.Measurements.Commands.Create;

using System;
using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Measurements.Errors;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.Errors;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class CreateMeasurementCommandHandler(
    IMeasurementRepository measurementRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateMeasurementCommand>
{
    private readonly IMeasurementRepository _measurementRepository = measurementRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        CreateMeasurementCommand request,
        CancellationToken cancellationToken)
    {
        var userIdResult = await ValidateUserIdAsync(request.UserId, cancellationToken);
        var nameResult = await userIdResult.OnSuccessAsync(
            async () => await ValidateNameAsync(
                request.Name, userIdResult.ValueOrDefault(), cancellationToken));

        var descriptionResult = Description.Create(request.Description);

        var unitResult = Enum.TryParse<MeasurementUnit>(request.Unit, out var unit)
            ? Result.Success(unit)
            : Result.Failure<MeasurementUnit>(DomainErrors.MeasurementUnit.Invalid);

        var measurementResult = await Result.Combine(
            nameResult,
            descriptionResult,
            unitResult)
            .OnSuccess(() => Measurement.Create(
                nameResult.ValueOrDefault(),
                descriptionResult.ValueOrDefault(),
                unit,
                userIdResult.ValueOrDefault()))
            .OnSuccessAsync(async m => await _measurementRepository.AddAsync(m, cancellationToken));

        if (measurementResult.IsFailure)
            return measurementResult;

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Measurement.CannotAddToDatabase);
        }

        return measurementResult;
    }

    private async Task<Result<Name>> ValidateNameAsync(
        string name,
        UserId userId,
        CancellationToken cancellationToken)
    {
        return await Name.Create(name).OnSuccessAsync(
            async n => await _measurementRepository.ValidateNameUniqueness(n, userId));
    }

    private async Task<Result<UserId>> ValidateUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await UserId.FromGuid(userId).OnSuccessAsync(
            async uId =>
                (await _userRepository.GetByIdAsync(uId, cancellationToken))
                .Map(u => u.Id));
    }
}
