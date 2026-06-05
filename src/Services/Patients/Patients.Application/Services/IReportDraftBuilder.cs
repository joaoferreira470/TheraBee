namespace Patients.Application.Services;

public interface IReportDraftBuilder
{
    Task<Report> BuildAsync(Guid patientId, Guid therapistId, CancellationToken cancellationToken);
}
