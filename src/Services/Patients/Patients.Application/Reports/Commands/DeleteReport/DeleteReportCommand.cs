using FluentValidation;

namespace Patients.Application.Reports.Commands.DeleteReport;

public record DeleteReportCommand(Guid ReportId) : ICommand<DeleteReportResult>;

public record DeleteReportResult(bool IsSuccess);

public class DeleteReportValidator : AbstractValidator<DeleteReportCommand>
{
    public DeleteReportValidator()
    {
        RuleFor(x => x.ReportId).NotEmpty().WithMessage("ReportId is required");
    }
}
