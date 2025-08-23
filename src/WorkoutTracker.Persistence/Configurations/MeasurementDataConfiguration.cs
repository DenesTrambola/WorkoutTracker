namespace WorkoutTracker.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.TypedIds;
using WorkoutTracker.Domain.Measurements.ValueObjects;
using WorkoutTracker.Domain.Shared.ValueObjects;

public sealed class MeasurementDataConfiguration : IEntityTypeConfiguration<MeasurementData>
{
    public void Configure([NotNull] EntityTypeBuilder<MeasurementData> builder)
    {
        builder.ToTable("MeasurementData");

        builder.HasKey(md => md.Id);

        builder.Property(md => md.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.IdValue,
                value => MeasurementDataId.FromGuid(value).ValueOrDefault())
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(md => md.Value)
            .HasColumnName("Value")
            .HasConversion(
                value => value.Value,
                value => MeasurementDataValue.Create(value).ValueOrDefault())
            .IsRequired();

        builder.Property(md => md.MeasuredOn)
            .HasColumnName("MeasuredOn")
            .IsRequired();

        builder.Property(md => md.Comment)
            .HasColumnName("Comment")
            .HasConversion(
                comment => comment.Text,
                value => Comment.Create(value).ValueOrDefault())
            .HasMaxLength(Comment.MaxLength)
            .IsRequired(false);

        builder.Property(md => md.MeasurementId)
            .HasColumnName("MeasurementId")
            .HasConversion(
                id => id.IdValue,
                value => MeasurementId.FromGuid(value).ValueOrDefault())
            .IsRequired();

        builder.HasOne<Measurement>()
            .WithMany(m => m.Data)
            .HasForeignKey(md => md.MeasurementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
