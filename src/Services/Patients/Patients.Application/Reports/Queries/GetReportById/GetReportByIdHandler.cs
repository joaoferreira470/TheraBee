namespace Patients.Application.Reports.Queries.GetReportById;

public class GetReportByIdHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetReportByIdQuery, GetReportByIdResult>
{
    public async Task<GetReportByIdResult> Handle(GetReportByIdQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var report = await dbContext.Reports
            .AsNoTracking()
            .FirstOrDefaultAsync(report => report.Id == query.ReportId && report.TherapistId == currentUserId, cancellationToken);

        if (report == null)
        {
            throw new ReportNotFoundException(query.ReportId);
        }

        return new GetReportByIdResult(report.ToReportDto());
    }
}
