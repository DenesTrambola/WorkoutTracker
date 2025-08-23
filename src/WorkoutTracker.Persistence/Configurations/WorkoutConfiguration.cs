namespace WorkoutTracker.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure([NotNull] EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("Workouts");

        builder.HasKey(w => w.Id);

        builder.Property(u => u.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.IdValue,
                value => WorkoutId.FromGuid(value).ValueOrDefault())
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(w => w.StartTime)
            .HasColumnName("StartTime")
            .IsRequired();

        builder.Property(w => w.EndTime)
            .HasColumnName("EndTime")
            .IsRequired();

        builder.Property(w => w.RestTimeBetweenExercises)
            .HasColumnName("RestTimeBetweenExercises")
            .IsRequired();

        builder.Property(w => w.Comment)
            .HasColumnName("Comment")
            .HasConversion(
                comment => comment.Text,
                value => Comment.Create(value).ValueOrDefault())
            .HasMaxLength(Comment.MaxLength)
            .IsRequired(false);

        builder.Property(w => w.UserId)
            .HasColumnName("UserId")
            .HasConversion(
                id => id.IdValue,
                value => UserId.FromGuid(value).ValueOrDefault())
            .IsRequired();

        builder.Property(w => w.RoutineId)
            .HasColumnName("RoutineId")
            .HasConversion(
                id => id.IdValue,
                value => RoutineId.FromGuid(value).ValueOrDefault())
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Routine>()
            .WithMany()
            .HasForeignKey(w => w.RoutineId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
