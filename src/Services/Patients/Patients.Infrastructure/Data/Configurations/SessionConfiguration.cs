using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Patients.Infrastructure.Data.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(session => session.Id);

        builder.Property(session => session.StartDateTime)
            .IsRequired();

        builder.Property(session => session.EndDateTime)
            .IsRequired();

        builder.Property(session => session.Type)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(session => session.Location)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(session => session.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(session => session.CancellationReason)
            .HasMaxLength(500);

        builder.Property(session => session.ClinicalSummary)
            .HasMaxLength(2000);

        builder.Property(session => session.ObjectivesWorked)
            .HasMaxLength(2000);

        builder.Property(session => session.ProgressRating)
            .HasMaxLength(200);

        builder.Property(session => session.Activities)
            .HasMaxLength(2000);

        builder.Property(session => session.PatientResponse)
            .HasMaxLength(2000);

        builder.Property(session => session.Difficulties)
            .HasMaxLength(2000);

        builder.Property(session => session.Recommendations)
            .HasMaxLength(2000);

        builder.Property(session => session.NextSteps)
            .HasMaxLength(2000);

        builder.Property(session => session.PatientId)
            .IsRequired();

        builder.Property(session => session.TherapistId)
            .IsRequired();

        builder.HasMany(session => session.SessionGoals)
            .WithOne()
            .HasForeignKey(sessionGoal => sessionGoal.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(session => session.SessionGoalAssessments)
            .WithOne()
            .HasForeignKey(assessment => assessment.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(session => new { session.TherapistId, session.PatientId, session.StartDateTime });
    }
}
