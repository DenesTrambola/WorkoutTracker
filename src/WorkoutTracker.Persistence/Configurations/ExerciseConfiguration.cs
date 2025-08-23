namespace WorkoutTracker.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure([NotNull] EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("Exercises");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.IdValue,
                value => ExerciseId.FromGuid(value).ValueOrDefault())
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(e => e.Name)
            .HasColumnName("Name")
            .HasConversion(
                name => name.Value,
                value => Name.Create(value).ValueOrDefault())
            .HasMaxLength(Name.MaxLength)
            .IsRequired();

        builder.Property(e => e.TargetMuscle)
            .HasColumnName("TargetMuscle")
            .HasConversion(
                muscle => muscle.Muscle,
                value => TargetMuscle.Create(value).ValueOrDefault())
            .HasMaxLength(TargetMuscle.MaxLength)
            .IsRequired();

        builder.Property(e => e.Visibility)
            .HasColumnName("Visibility")
            .HasConversion(
                visibility => visibility.IsPublic,
                value => Visibility.Create(value).ValueOrDefault())
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasColumnName("UserId")
            .HasConversion(
                id => id.IdValue,
                value => UserId.FromGuid(value).ValueOrDefault())
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
