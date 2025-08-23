namespace WorkoutTracker.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;

public sealed class MeasurementConfiguration : IEntityTypeConfiguration<Measurement>
{
    public void Configure([NotNull] EntityTypeBuilder<Measurement> builder)
    {
        builder.ToTable("Measurements");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.IdValue,
                value => MeasurementId.FromGuid(value).ValueOrDefault())
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(m => m.Name)
            .HasColumnName("Name")
            .HasConversion(
                name => name.Value,
                value => Name.Create(value).ValueOrDefault())
            .HasMaxLength(Name.MaxLength)
            .IsRequired();

        builder.Property(m => m.Description)
            .HasColumnName("Description")
            .HasConversion(
                description => description.Text,
                value => Description.Create(value).ValueOrDefault())
            .HasMaxLength(Description.MaxLength)
            .IsRequired(false);

        builder.Property(m => m.Unit)
            .HasColumnName("Unit")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(m => m.UserId)
            .HasColumnName("UserId")
            .HasConversion(
                id => id.IdValue,
                value => UserId.FromGuid(value).ValueOrDefault())
            .IsRequired();

        builder.HasMany(m => m.Data)
            .WithOne()
            .HasForeignKey(md => md.MeasurementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
