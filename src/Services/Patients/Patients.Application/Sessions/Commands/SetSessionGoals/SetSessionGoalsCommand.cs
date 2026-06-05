using FluentValidation;

namespace Patients.Application.Sessions.Commands.SetSessionGoals;

public record SetSessionGoalsCommand(Guid SessionId, SetSessionGoalsDto Goals)
    : ICommand<SetSessionGoalsResult>;

public record SetSessionGoalsResult(bool IsSuccess);

public class SetSessionGoalsValidator : AbstractValidator<SetSessionGoalsCommand>
{
    public SetSessionGoalsValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty().WithMessage("SessionId is required");
        RuleFor(x => x.Goals.GoalIds)
            .Must(goalIds => goalIds != null)
            .WithMessage("GoalIds is required");
    }
}
