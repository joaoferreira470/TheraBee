using FluentValidation;

namespace Patients.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password)
    : ICommand<LoginResult>;

public record LoginResult(AuthDto Auth);

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}

