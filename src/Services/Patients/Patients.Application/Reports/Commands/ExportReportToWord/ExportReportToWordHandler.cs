using Patients.Application.Reports.Services;

namespace Patients.Application.Reports.Commands.ExportReportToWord;

public class ExportReportToWordHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<ExportReportToWordCommand, ExportReportToWordResult>
{
    public async Task<ExportReportToWordResult> Handle(ExportReportToWordCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var report = await dbContext.Reports
            .FirstOrDefaultAsync(report => report.Id == command.ReportId && report.TherapistId == currentUserId, cancellationToken);

        if (report == null)
        {
            throw new ReportNotFoundException(command.ReportId);
        }

        var export = ReportDocumentExporter.ExportWord(report);
        report.SetWordExport(export.Content, export.FileName);

        dbContext.Reports.Update(report);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ExportReportToWordResult(export.Content, export.ContentType, export.FileName);
    }
}
