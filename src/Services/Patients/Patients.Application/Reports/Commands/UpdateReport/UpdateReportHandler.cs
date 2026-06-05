namespace Patients.Application.Reports.Commands.UpdateReport;

public class UpdateReportHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<UpdateReportCommand, UpdateReportResult>
{
    public async Task<UpdateReportResult> Handle(UpdateReportCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var report = await dbContext.Reports
            .FirstOrDefaultAsync(report => report.Id == command.ReportId && report.TherapistId == currentUserId, cancellationToken);

        if (report == null)
        {
            throw new ReportNotFoundException(command.ReportId);
        }

        report.Update(
            title: command.Report.Title,
            periodStart: command.Report.PeriodStart,
            periodEnd: command.Report.PeriodEnd,
            patientSnapshot: command.Report.PatientSnapshot,
            executiveSummary: command.Report.ExecutiveSummary,
            attendanceSummary: command.Report.AttendanceSummary,
            goalProgressSummary: command.Report.GoalProgressSummary,
            sessionSummary: command.Report.SessionSummary,
            recommendations: command.Report.Recommendations,
            additionalNotes: command.Report.AdditionalNotes);

        dbContext.Reports.Update(report);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateReportResult(true);
    }
}
