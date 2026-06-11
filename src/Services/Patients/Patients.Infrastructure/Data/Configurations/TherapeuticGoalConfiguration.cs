using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patients.Domain.Models;

namespace Patients.Infrastructure.Data.Configurations;

public class TherapeuticGoalConfiguration : IEntityTypeConfiguration<TherapeuticGoal>
{
    public void Configure(EntityTypeBuilder<TherapeuticGoal> builder)
    {
        builder.HasKey(goal => goal.Id);

        builder.Property(goal => goal.Type)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(goal => goal.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(goal => goal.Priority)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(goal => goal.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(goal => goal.PatientId)
            .IsRequired();

        builder.Property(goal => goal.TherapistId)
            .IsRequired();

        builder.HasIndex(goal => new { goal.TherapistId, goal.PatientId });
    }
}
