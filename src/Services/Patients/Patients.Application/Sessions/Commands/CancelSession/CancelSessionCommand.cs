using FluentValidation;

namespace Patients.Application.Sessions.Commands.CancelSession;

public record CancelSessionCommand(Guid SessionId, CancelSessionDto Session)
    : ICommand<CancelSessionResult>;

public record CancelSessionResult(bool IsSuccess);

public class CancelSessionValidator : AbstractValidator<CancelSessionCommand>
{
    public CancelSessionValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty().WithMessage("SessionId is required");
        RuleFor(x => x.Session.CancellationReason)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("CancellationReason is required");
    }
}
