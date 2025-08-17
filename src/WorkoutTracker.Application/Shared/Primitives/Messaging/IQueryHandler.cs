namespace WorkoutTracker.Application.Shared.Primitives.Messaging;

using MediatR;
using WorkoutTracker.Domain.Shared.Results;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
