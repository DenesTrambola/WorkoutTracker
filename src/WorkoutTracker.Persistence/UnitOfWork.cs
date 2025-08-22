namespace WorkoutTracker.Persistence;

using Microsoft.EntityFrameworkCore.Storage;
using WorkoutTracker.Domain.Shared.Primitives;
using WorkoutTracker.Domain.Users;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IUserRepository Users { get; }

    public UnitOfWork(
        AppDbContext context,
        IUserRepository userRepository)
    {
        _context = context;
        Users = userRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}

