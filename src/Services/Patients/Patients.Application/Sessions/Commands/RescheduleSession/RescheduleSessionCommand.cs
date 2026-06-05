using FluentValidation;

namespace Patients.Application.Sessions.Commands.RescheduleSession;

public record RescheduleSessionCommand(Guid SessionId, RescheduleSessionDto Session)
    : ICommand<RescheduleSessionResult>;

public record RescheduleSessionResult(bool IsSuccess);

public class RescheduleSessionValidator : AbstractValidator<RescheduleSessionCommand>
{
    public RescheduleSessionValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty().WithMessage("SessionId is required");
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
