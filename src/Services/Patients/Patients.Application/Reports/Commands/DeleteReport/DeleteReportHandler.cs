namespace Patients.Application.Reports.Commands.DeleteReport;

public class DeleteReportHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<DeleteReportCommand, DeleteReportResult>
{
    public async Task<DeleteReportResult> Handle(DeleteReportCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;

        var report = await dbContext.Reports
            .FirstOrDefaultAsync(report => report.Id == command.ReportId && report.TherapistId == currentUserId, cancellationToken);

        if (report is null)
        {
            throw new ReportNotFoundException(command.ReportId);
        }

        dbContext.Reports.Remove(report);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteReportResult(true);
    }
}
