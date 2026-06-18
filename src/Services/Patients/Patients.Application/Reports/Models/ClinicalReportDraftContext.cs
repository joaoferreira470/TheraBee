namespace Patients.Application.Reports.Models;

public record ClinicalReportDraftContext(
    PatientDto Patient,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    IReadOnlyList<TherapeuticGoal> Goals,
    IReadOnlyList<Session> Sessions,
    PatientProgressDashboardDto Dashboard);
