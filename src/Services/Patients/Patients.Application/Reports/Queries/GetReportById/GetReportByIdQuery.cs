namespace Patients.Application.Reports.Queries.GetReportById;

public record GetReportByIdQuery(Guid ReportId)
    : IQuery<GetReportByIdResult>;

public record GetReportByIdResult(ReportDto Report);
