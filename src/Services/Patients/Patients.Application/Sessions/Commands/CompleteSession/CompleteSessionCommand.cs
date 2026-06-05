using FluentValidation;

namespace Patients.Application.Sessions.Commands.CompleteSession;

public record CompleteSessionCommand(Guid SessionId, CompleteSessionDto Session)
    : ICommand<CompleteSessionResult>;

public record CompleteSessionResult(bool IsSuccess);

public class CompleteSessionValidator : AbstractValidator<CompleteSessionCommand>
{
    public CompleteSessionValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty().WithMessage("SessionId is required");
        RuleFor(x => x.Session.ClinicalSummary).NotEmpty().MaximumLength(2000).WithMessage("ClinicalSummary is required");
        RuleFor(x => x.Session.ObjectivesWorked).NotEmpty().MaximumLength(2000).WithMessage("ObjectivesWorked is required");
        RuleFor(x => x.Session.ProgressRating).NotEmpty().MaximumLength(200).WithMessage("ProgressRating is required");
        RuleFor(x => x.Session.Activities).NotEmpty().MaximumLength(2000).WithMessage("Activities is required");
        RuleFor(x => x.Session.PatientResponse).NotEmpty().MaximumLength(2000).WithMessage("PatientResponse is required");
        RuleFor(x => x.Session.Difficulties).NotEmpty().MaximumLength(2000).WithMessage("Difficulties is required");
        RuleFor(x => x.Session.Recommendations).NotEmpty().MaximumLength(2000).WithMessage("Recommendations is required");
        RuleFor(x => x.Session.NextSteps).NotEmpty().MaximumLength(2000).WithMessage("NextSteps is required");
    }
}
