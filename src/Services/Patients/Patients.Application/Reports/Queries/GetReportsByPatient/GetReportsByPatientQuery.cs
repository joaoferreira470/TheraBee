namespace Patients.Application.Reports.Queries.GetReportsByPatient;

public record GetReportsByPatientQuery(Guid PatientId)
    : IQuery<GetReportsByPatientResult>;

public record GetReportsByPatientResult(IEnumerable<ReportDto> Reports);
