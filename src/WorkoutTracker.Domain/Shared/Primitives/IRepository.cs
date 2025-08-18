namespace WorkoutTracker.Domain.Shared.Primitives;

using WorkoutTracker.Domain.Shared.Results;

public interface IRepository<T, TId>
    where T : Entity<TId>
    where TId : StronglyTypedId<Guid>
{
    Task<Result<IEnumerable<T>>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Result<T>> GetByIdAsync(
        TId id,
        CancellationToken cancellationToken = default);

    Task<Result<T>> AddAsync(
        T entity,
        CancellationToken cancellationToken = default);

    Task<Result<T>> Update(
        T entity,
        CancellationToken cancellationToken = default);

    Task<Result> Delete(
        TId id,
        CancellationToken cancellationToken = default);
}
