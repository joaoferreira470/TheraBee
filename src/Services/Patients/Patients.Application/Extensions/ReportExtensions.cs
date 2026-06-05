namespace Patients.Application.Extensions;

public static class ReportExtensions
{
    public static ReportDto ToReportDto(this Report report)
    {
        return new ReportDto(
            Id: report.Id,
            PatientId: report.PatientId,
            TherapistId: report.TherapistId,
            Title: report.Title,
            PeriodStart: report.PeriodStart,
            PeriodEnd: report.PeriodEnd,
            PatientSnapshot: report.PatientSnapshot,
            ExecutiveSummary: report.ExecutiveSummary,
            AttendanceSummary: report.AttendanceSummary,
            GoalProgressSummary: report.GoalProgressSummary,
            SessionSummary: report.SessionSummary,
            Recommendations: report.Recommendations,
            AdditionalNotes: report.AdditionalNotes,
            HasPdfExport: report.PdfContent is not null,
            HasWordExport: report.WordContent is not null,
            PdfFileName: report.PdfFileName,
            WordFileName: report.WordFileName,
            PdfExportedAt: report.PdfExportedAt,
            WordExportedAt: report.WordExportedAt,
            CreatedAt: report.CreatedAt);
    }

    public static IEnumerable<ReportDto> ToReportDtoList(this IEnumerable<Report> reports)
    {
        return reports.Select(ToReportDto);
    }
}
