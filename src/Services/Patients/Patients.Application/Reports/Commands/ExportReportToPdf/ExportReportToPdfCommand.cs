using FluentValidation;

namespace Patients.Application.Reports.Commands.ExportReportToPdf;

public record ExportReportToPdfCommand(Guid ReportId)
    : ICommand<ExportReportToPdfResult>;

public record ExportReportToPdfResult(byte[] Content, string ContentType, string FileName);

public class ExportReportToPdfValidator : AbstractValidator<ExportReportToPdfCommand>
{
    public ExportReportToPdfValidator()
    {
        RuleFor(x => x.ReportId).NotEmpty().WithMessage("ReportId is required");
    }
}
