using FluentValidation;

namespace Patients.Application.Reports.Commands.CreateReportDraft;

public record CreateReportDraftCommand(Guid PatientId)
    : ICommand<CreateReportDraftResult>;

public record CreateReportDraftResult(Guid Id);

public class CreateReportDraftValidator : AbstractValidator<CreateReportDraftCommand>
{
    public CreateReportDraftValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("PatientId is required");
    }
}
