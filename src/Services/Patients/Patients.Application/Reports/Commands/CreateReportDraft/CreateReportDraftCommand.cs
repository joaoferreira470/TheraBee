using FluentValidation;

namespace Patients.Application.Reports.Commands.CreateReportDraft;

public record CreateReportDraftCommand(Guid PatientId, DateTime PeriodStart, DateTime PeriodEnd)
    : ICommand<CreateReportDraftResult>;

public record CreateReportDraftResult(Guid Id);

public class CreateReportDraftValidator : AbstractValidator<CreateReportDraftCommand>
{
    public CreateReportDraftValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("PatientId is required");
        RuleFor(x => x.PeriodStart).NotEqual(default(DateTime)).WithMessage("PeriodStart is required");
        RuleFor(x => x.PeriodEnd).NotEqual(default(DateTime)).WithMessage("PeriodEnd is required");
        RuleFor(x => x.PeriodEnd)
            .GreaterThanOrEqualTo(x => x.PeriodStart)
            .WithMessage("PeriodEnd must be greater than or equal to PeriodStart");
    }
}
