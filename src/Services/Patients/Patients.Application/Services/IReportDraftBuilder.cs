namespace Patients.Application.Services;

public interface IReportDraftBuilder
{
    Task<Report> BuildAsync(Guid patientId, Guid therapistId, DateTime periodStart, DateTime periodEnd, CancellationToken cancellationToken);
}
