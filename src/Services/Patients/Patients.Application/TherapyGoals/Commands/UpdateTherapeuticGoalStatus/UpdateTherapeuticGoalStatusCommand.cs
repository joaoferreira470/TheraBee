using FluentValidation;

namespace Patients.Application.TherapyGoals.Commands.UpdateTherapeuticGoalStatus;

public record UpdateTherapeuticGoalStatusCommand(Guid GoalId, TherapeuticGoalStatus Status)
    : ICommand<UpdateTherapeuticGoalStatusResult>;

public record UpdateTherapeuticGoalStatusResult(bool IsSuccess);

public class UpdateTherapeuticGoalStatusValidator : AbstractValidator<UpdateTherapeuticGoalStatusCommand>
{
    public UpdateTherapeuticGoalStatusValidator()
    {
        RuleFor(x => x.GoalId).NotEmpty().WithMessage("GoalId is required");
        RuleFor(x => x.Status).IsInEnum().WithMessage("Status is required");
    }
}
