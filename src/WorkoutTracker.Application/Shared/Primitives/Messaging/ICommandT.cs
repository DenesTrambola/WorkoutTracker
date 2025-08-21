namespace WorkoutTracker.Application.Shared.Primitives.Messaging;

using MediatR;
using WorkoutTracker.Domain.Shared.Results;

public interface ICommand<TResponse>
    : IRequest<Result<TResponse>>
{
}
