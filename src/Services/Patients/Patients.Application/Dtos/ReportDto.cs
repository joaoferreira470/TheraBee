namespace Patients.Application.Dtos;

public record ReportDto(
    Guid Id,
    Guid PatientId,
    Guid TherapistId,
    string Title,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    string PatientSnapshot,
    string ExecutiveSummary,
    string AttendanceSummary,
    string GoalProgressSummary,
    string SessionSummary,
    string Recommendations,
    string? AdditionalNotes,
    bool HasPdfExport,
    bool HasWordExport,
    string? PdfFileName,
    string? WordFileName,
    DateTime? PdfExportedAt,
    DateTime? WordExportedAt,
    DateTime? CreatedAt);
