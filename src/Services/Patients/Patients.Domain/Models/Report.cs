namespace Patients.Domain.Models;

public class Report : Entity<Guid>
{
    public Guid PatientId { get; set; }
    public Guid TherapistId { get; set; }
    public string Title { get; set; } = default!;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public string PatientSnapshot { get; set; } = default!;
    public string ExecutiveSummary { get; set; } = default!;
    public string AttendanceSummary { get; set; } = default!;
    public string GoalProgressSummary { get; set; } = default!;
    public string SessionSummary { get; set; } = default!;
    public string Recommendations { get; set; } = default!;
    public string? AdditionalNotes { get; set; }
    public byte[]? PdfContent { get; set; }
    public string? PdfFileName { get; set; }
    public DateTime? PdfExportedAt { get; set; }
    public byte[]? WordContent { get; set; }
    public string? WordFileName { get; set; }
    public DateTime? WordExportedAt { get; set; }
    public DateTime GeneratedAt { get; set; }

    public static Report CreateDraft(
        Guid id,
        Guid patientId,
        Guid therapistId,
        string title,
        DateTime periodStart,
        DateTime periodEnd,
        string patientSnapshot,
        string executiveSummary,
        string attendanceSummary,
        string goalProgressSummary,
        string sessionSummary,
        string recommendations,
        string? additionalNotes)
    {
        return new Report
        {
            Id = id,
            PatientId = patientId,
            TherapistId = therapistId,
            Title = title.Trim(),
            PeriodStart = periodStart,
            PeriodEnd = periodEnd,
            PatientSnapshot = patientSnapshot.Trim(),
            ExecutiveSummary = executiveSummary.Trim(),
            AttendanceSummary = attendanceSummary.Trim(),
            GoalProgressSummary = goalProgressSummary.Trim(),
            SessionSummary = sessionSummary.Trim(),
            Recommendations = recommendations.Trim(),
            AdditionalNotes = string.IsNullOrWhiteSpace(additionalNotes) ? null : additionalNotes.Trim(),
            GeneratedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
        };
    }

    public void Update(
        string title,
        DateTime periodStart,
        DateTime periodEnd,
        string patientSnapshot,
        string executiveSummary,
        string attendanceSummary,
        string goalProgressSummary,
        string sessionSummary,
        string recommendations,
        string? additionalNotes)
    {
        Title = title.Trim();
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        PatientSnapshot = patientSnapshot.Trim();
        ExecutiveSummary = executiveSummary.Trim();
        AttendanceSummary = attendanceSummary.Trim();
        GoalProgressSummary = goalProgressSummary.Trim();
        SessionSummary = sessionSummary.Trim();
        Recommendations = recommendations.Trim();
        AdditionalNotes = string.IsNullOrWhiteSpace(additionalNotes) ? null : additionalNotes.Trim();
    }

    public void SetPdfExport(byte[] content, string fileName)
    {
        PdfContent = content;
        PdfFileName = fileName;
        PdfExportedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    }

    public void SetWordExport(byte[] content, string fileName)
    {
        WordContent = content;
        WordFileName = fileName;
        WordExportedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    }
}
