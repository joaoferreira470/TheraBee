namespace Patients.Application.Dtos;

public record UpdateReportDto(
    string Title,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    string PatientSnapshot,
    string ExecutiveSummary,
    string AttendanceSummary,
    string GoalProgressSummary,
    string SessionSummary,
    string Recommendations,
    string? AdditionalNotes);
