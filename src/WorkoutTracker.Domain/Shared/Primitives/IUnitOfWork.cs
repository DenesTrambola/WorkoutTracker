namespace WorkoutTracker.Domain.Shared.Primitives;

using WorkoutTracker.Domain.Users;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
