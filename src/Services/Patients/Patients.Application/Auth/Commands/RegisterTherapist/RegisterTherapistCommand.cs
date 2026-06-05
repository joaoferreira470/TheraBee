using FluentValidation;

namespace Patients.Application.Auth.Commands.RegisterTherapist;

public record RegisterTherapistCommand(
    string Name,
    string Email,
    string Password,
    string Profession)
    : ICommand<RegisterTherapistResult>;

public record RegisterTherapistResult(AuthDto Auth);

public class RegisterTherapistCommandValidator : AbstractValidator<RegisterTherapistCommand>
{
    public RegisterTherapistCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required");
        RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password must have at least 8 characters");
        RuleFor(x => x.Profession).NotEmpty().WithMessage("Profession is required");
    }
}

