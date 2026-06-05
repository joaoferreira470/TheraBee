using FluentValidation;

namespace Patients.Application.Reports.Commands.ExportReportToWord;

public record ExportReportToWordCommand(Guid ReportId)
    : ICommand<ExportReportToWordResult>;

public record ExportReportToWordResult(byte[] Content, string ContentType, string FileName);

public class ExportReportToWordValidator : AbstractValidator<ExportReportToWordCommand>
{
    public ExportReportToWordValidator()
    {
        RuleFor(x => x.ReportId).NotEmpty().WithMessage("ReportId is required");
    }
}
