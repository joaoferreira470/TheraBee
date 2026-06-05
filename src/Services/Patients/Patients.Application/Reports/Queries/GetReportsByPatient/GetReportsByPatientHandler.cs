namespace Patients.Application.Reports.Queries.GetReportsByPatient;

public class GetReportsByPatientHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetReportsByPatientQuery, GetReportsByPatientResult>
{
    public async Task<GetReportsByPatientResult> Handle(GetReportsByPatientQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patientExists = await dbContext.Patients.AnyAsync(
            patient => patient.Id == PatientId.Of(query.PatientId) && patient.TherapistId == currentUserId,
            cancellationToken);

        if (!patientExists)
        {
            throw new PatientNotFoundException(query.PatientId);
        }

        var reports = await dbContext.Reports
            .AsNoTracking()
            .Where(report => report.PatientId == query.PatientId && report.TherapistId == currentUserId)
            .OrderByDescending(report => report.GeneratedAt)
            .ToListAsync(cancellationToken);

        return new GetReportsByPatientResult(reports.ToReportDtoList());
    }
}
