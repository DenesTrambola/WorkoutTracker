namespace WorkoutTracker.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Routines.ValueObjects;
using WorkoutTracker.Domain.Shared.ValueObjects;

public sealed class RoutineExerciseConfiguration : IEntityTypeConfiguration<RoutineExercise>
{
    public void Configure([NotNull] EntityTypeBuilder<RoutineExercise> builder)
    {
        builder.ToTable("RoutineExercises");

        builder.HasKey(re => re.Id);

        builder.Property(re => re.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.IdValue,
                value => RoutineExerciseId.FromGuid(value).ValueOrDefault())
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(re => re.SetCount)
            .HasColumnName("SetCount")
            .IsRequired();

        builder.Property(re => re.RepCount)
            .HasColumnName("RepCount")
            .IsRequired();

        builder.Property(re => re.RestTimeBetweenSets)
            .HasColumnName("RestTimeBetweenSets")
            .IsRequired();

        builder.Property(re => re.Comment)
            .HasColumnName("Comment")
            .HasConversion(
                comment => comment.Text,
                value => Comment.Create(value).ValueOrDefault())
            .HasMaxLength(Comment.MaxLength)
            .IsRequired(false);

        builder.Property(re => re.Position)
            .HasColumnName("Position")
            .HasConversion(
                position => position.Value,
                value => ExercisePosition.Create(value).ValueOrDefault())
            .IsRequired();

        builder.Property(re => re.RoutineId)
            .HasColumnName("RoutineId")
            .HasConversion(
                id => id.IdValue,
                value => RoutineId.FromGuid(value).ValueOrDefault())
            .IsRequired();

        builder.Property(re => re.ExerciseId)
            .HasColumnName("ExerciseId")
            .HasConversion(
                id => id.IdValue,
                value => ExerciseId.FromGuid(value).ValueOrDefault())
            .IsRequired();

        builder.HasOne<Routine>()
            .WithMany(r => r.RoutineExercises)
            .HasForeignKey(re => re.RoutineId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Exercise>()
            .WithMany()
            .HasForeignKey(re => re.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
