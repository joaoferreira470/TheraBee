using FluentValidation;

namespace Patients.Application.Sessions.Commands.AddSessionGoals;

public record AddSessionGoalsCommand(Guid SessionId, AddSessionGoalsDto Goals)
    : ICommand<AddSessionGoalsResult>;

public record AddSessionGoalsResult(bool IsSuccess);

public class AddSessionGoalsValidator : AbstractValidator<AddSessionGoalsCommand>
{
    public AddSessionGoalsValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty().WithMessage("SessionId is required");
        RuleFor(x => x.Goals.GoalIds)
            .NotNull()
            .WithMessage("GoalIds are required");
        RuleFor(x => x.Goals.GoalIds)
            .Must(goalIds => goalIds != null && goalIds.Any())
            .WithMessage("At least one GoalId is required");
    }
}
