namespace Patients.Application.Reports.Models;

public record ClinicalReportNarrative(
    string ExecutiveSummary,
    string AttendanceSummary,
    string GoalProgressSummary,
    string SessionSummary,
    string Recommendations,
    string? AdditionalNotes);
