namespace WorkoutTracker.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class RoutineConfiguration : IEntityTypeConfiguration<Routine>
{
    public void Configure([NotNull] EntityTypeBuilder<Routine> builder)
    {
        builder.ToTable("Routines");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.IdValue,
                value => RoutineId.FromGuid(value).ValueOrDefault())
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(r => r.Name)
            .HasColumnName("Name")
            .HasConversion(
                name => name.Value,
                value => Name.Create(value).ValueOrDefault())
            .HasMaxLength(Name.MaxLength)
            .IsRequired();

        builder.Property(r => r.Description)
            .HasColumnName("Description")
            .HasConversion(
                description => description.Text,
                value => Description.Create(value).ValueOrDefault())
            .HasMaxLength(Description.MaxLength)
            .IsRequired(false);

        builder.Property(r => r.UserId)
            .HasColumnName("UserId")
            .HasConversion(
                id => id.IdValue,
                value => UserId.FromGuid(value).ValueOrDefault())
            .IsRequired();

        builder.HasMany(r => r.RoutineExercises)
            .WithOne()
            .HasForeignKey(re => re.RoutineId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
