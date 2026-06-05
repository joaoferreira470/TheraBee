using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Patients.Infrastructure.Data.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(report => report.Id);

        builder.Property(report => report.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(report => report.PeriodStart)
            .IsRequired();

        builder.Property(report => report.PeriodEnd)
            .IsRequired();

        builder.Property(report => report.PatientSnapshot)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(report => report.ExecutiveSummary)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(report => report.AttendanceSummary)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(report => report.GoalProgressSummary)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(report => report.SessionSummary)
            .HasMaxLength(6000)
            .IsRequired();

        builder.Property(report => report.Recommendations)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(report => report.AdditionalNotes)
            .HasMaxLength(4000);

        builder.Property(report => report.PdfFileName)
            .HasMaxLength(255);

        builder.Property(report => report.WordFileName)
            .HasMaxLength(255);

        builder.Property(report => report.PatientId)
            .IsRequired();

        builder.Property(report => report.TherapistId)
            .IsRequired();

        builder.HasIndex(report => new { report.TherapistId, report.PatientId, report.GeneratedAt });
    }
}
