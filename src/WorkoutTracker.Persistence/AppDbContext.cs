namespace WorkoutTracker.Persistence;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Users;

public class AppDbContext : DbContext
{
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<Measurement> Measurements => Set<Measurement>();
    public DbSet<MeasurementData> MeasurementData => Set<MeasurementData>();
    public DbSet<Routine> Routines => Set<Routine>();
    public DbSet<RoutineExercise> RoutineExercises => Set<RoutineExercise>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Workout> Workouts => Set<Workout>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(PersistenceAssemblyReference.Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
