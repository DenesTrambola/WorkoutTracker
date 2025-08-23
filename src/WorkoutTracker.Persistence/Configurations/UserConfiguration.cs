namespace WorkoutTracker.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure([NotNull] EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.IdValue,
                value => UserId.FromGuid(value).ValueOrDefault())
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(u => u.Username)
            .HasColumnName("Username")
            .HasConversion(
                username => username.Value,
                value => Username.Create(value).ValueOrDefault())
            .HasMaxLength(Username.MaxLength)
            .IsRequired();
        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("PasswordHash")
            .HasConversion(
                builder => builder.Value, 
                value => PasswordHash.Create(value).ValueOrDefault())
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("Email")
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).ValueOrDefault())
            .HasMaxLength(Email.MaxLength)
            .IsRequired();
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.OwnsOne(builder => builder.FullName, fullNameBuilder =>
        {
            fullNameBuilder.Property(fn => fn.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(FullName.MaxLength)
                .IsRequired();

            fullNameBuilder.Property(fn => fn.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(FullName.MaxLength)
                .IsRequired();
        });

        builder.Property(u => u.Gender)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(u => u.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(u => u.BirthDate)
            .HasColumnName("BirthDate")
            .HasConversion(
                date => date.ToDateTime(TimeOnly.MinValue),
                value => DateOnly.FromDateTime(value))
            .IsRequired();

        builder.Property(u => u.CreatedOn)
            .HasColumnName("CreatedOn")
            .IsRequired();

        builder.OwnsMany(u => u.RoutineIds, routineBuilder =>
        {
            routineBuilder.ToTable("UserRoutineIds");

            routineBuilder.WithOwner().HasForeignKey("UserId");

            routineBuilder.Property(r => r.IdValue)
                .HasColumnName("RoutineId")
                .IsRequired();
        });

        builder.OwnsMany(u => u.ExerciseIds, exerciseBuilder =>
        {
            exerciseBuilder.ToTable("UserExerciseIds");

            exerciseBuilder.WithOwner().HasForeignKey("UserId");

            exerciseBuilder.Property(e => e.IdValue)
                .HasColumnName("ExerciseId")
                .IsRequired();
        });

        builder.OwnsMany(u => u.MeasurementIds, measurementBuilder =>
        {
            measurementBuilder.ToTable("UserMeasurementIds");

            measurementBuilder.WithOwner().HasForeignKey("UserId");

            measurementBuilder.Property(m => m.IdValue)
                .HasColumnName("MeasurementId")
                .IsRequired();
        });

        builder.HasMany<Workout>()
            .WithOne()
            .HasForeignKey(w => w.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
