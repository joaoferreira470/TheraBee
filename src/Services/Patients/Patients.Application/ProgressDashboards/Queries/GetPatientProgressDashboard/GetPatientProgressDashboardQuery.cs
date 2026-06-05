namespace Patients.Application.ProgressDashboards.Queries.GetPatientProgressDashboard;

public record GetPatientProgressDashboardQuery(Guid PatientId)
    : IQuery<GetPatientProgressDashboardResult>;

public record GetPatientProgressDashboardResult(PatientProgressDashboardDto Dashboard);
