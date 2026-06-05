using FluentValidation;

namespace Patients.Application.Sessions.Commands.CreateSession;

public record CreateSessionCommand(Guid PatientId, CreateSessionDto Session)
    : ICommand<CreateSessionResult>;

public record CreateSessionResult(Guid Id);

public class CreateSessionValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("PatientId is required");
        RuleFor(x => x.Session.StartDateTime)
            .NotEmpty()
            .WithMessage("StartDateTime is required");
        RuleFor(x => x.Session.EndDateTime)
            .NotEmpty()
            .WithMessage("EndDateTime is required");
        RuleFor(x => x.Session.EndDateTime)
            .GreaterThan(x => x.Session.StartDateTime)
            .WithMessage("EndDateTime must be after StartDateTime");
        RuleFor(x => x.Session.Type)
            .IsInEnum()
            .WithMessage("Type is required");
        RuleFor(x => x.Session.Location)
            .NotEmpty()
            .MaximumLength(150)
            .WithMessage("Location is required");
    }
}
