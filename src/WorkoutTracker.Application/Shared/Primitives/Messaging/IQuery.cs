namespace WorkoutTracker.Application.Shared.Primitives.Messaging;

using MediatR;
using WorkoutTracker.Domain.Shared.Results;

public interface IQuery<TResponse>
    : IRequest<Result<TResponse>>
{
}
