namespace Patients.Application.Services;

public interface IPatientProgressDashboardBuilder
{
    Task<PatientProgressDashboardDto> BuildAsync(Guid patientId, Guid therapistId, CancellationToken cancellationToken, DateTime? periodStart = null, DateTime? periodEnd = null);
}
