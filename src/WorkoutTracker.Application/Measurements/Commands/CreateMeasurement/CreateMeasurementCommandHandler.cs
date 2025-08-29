namespace WorkoutTracker.Application.Measurements.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.Enums;
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
            async () => await CreateAndValidateNameAsync(
                request.Name, userIdResult.ValueOrDefault(), cancellationToken));

        var descriptionResult = Description.Create(request.Description);

        var unit = (MeasurementUnit)request.Unit;

        var measurementResult = await Result.Combine(
            nameResult,
            descriptionResult)
            .OnSuccess(() => Measurement.Create(
                nameResult.ValueOrDefault(),
                descriptionResult.ValueOrDefault(),
                unit,
                userIdResult.ValueOrDefault()))
            .OnSuccessAsync(async m => await _measurementRepository.AddAsync(m, cancellationToken));

        return measurementResult.OnSuccess(_ =>
            _unitOfWork.SaveChangesAsync(cancellationToken));
    }

    private async Task<Result<Name>> CreateAndValidateNameAsync(
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
