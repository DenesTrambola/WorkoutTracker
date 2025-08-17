namespace WorkoutTracker.Application.Shared.Primitives.Messaging;

using MediatR;
using WorkoutTracker.Domain.Shared.Results;

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
