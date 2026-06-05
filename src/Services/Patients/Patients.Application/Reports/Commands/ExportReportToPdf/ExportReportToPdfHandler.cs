using Patients.Application.Reports.Services;

namespace Patients.Application.Reports.Commands.ExportReportToPdf;

public class ExportReportToPdfHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<ExportReportToPdfCommand, ExportReportToPdfResult>
{
    public async Task<ExportReportToPdfResult> Handle(ExportReportToPdfCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var report = await dbContext.Reports
            .FirstOrDefaultAsync(report => report.Id == command.ReportId && report.TherapistId == currentUserId, cancellationToken);

        if (report == null)
        {
            throw new ReportNotFoundException(command.ReportId);
        }

        var export = ReportDocumentExporter.ExportPdf(report);
        report.SetPdfExport(export.Content, export.FileName);

        dbContext.Reports.Update(report);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ExportReportToPdfResult(export.Content, export.ContentType, export.FileName);
    }
}
