using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Patients.Infrastructure.Data.Configurations;

public class SessionGoalAssessmentConfiguration : IEntityTypeConfiguration<SessionGoalAssessment>
{
    public void Configure(EntityTypeBuilder<SessionGoalAssessment> builder)
    {
        builder.HasKey(assessment => assessment.Id);

        builder.Property(assessment => assessment.SessionId)
            .IsRequired();

        builder.Property(assessment => assessment.TherapeuticGoalId)
            .IsRequired();

        builder.Property(assessment => assessment.TherapistId)
            .IsRequired();

        builder.Property(assessment => assessment.Score)
            .IsRequired();

        builder.Property(assessment => assessment.ClinicalNotes)
            .HasMaxLength(2000);

        builder.HasIndex(assessment => new { assessment.SessionId, assessment.TherapeuticGoalId })
            .IsUnique();
    }
}
