using FluentValidation;

namespace Patients.Application.TherapyGoals.Commands.UpdateTherapeuticGoal;

public record UpdateTherapeuticGoalCommand(Guid GoalId, UpdateTherapeuticGoalDto Goal)
    : ICommand<UpdateTherapeuticGoalResult>;

public record UpdateTherapeuticGoalResult(bool IsSuccess);

public class UpdateTherapeuticGoalValidator : AbstractValidator<UpdateTherapeuticGoalCommand>
{
    public UpdateTherapeuticGoalValidator()
    {
        RuleFor(x => x.GoalId).NotEmpty().WithMessage("GoalId is required");
        RuleFor(x => x.Goal.Type).IsInEnum().WithMessage("Type is required");
        RuleFor(x => x.Goal.Description).NotEmpty().MaximumLength(500).WithMessage("Description is required");
        RuleFor(x => x.Goal.Area).NotEmpty().MaximumLength(150).WithMessage("Area is required");
        RuleFor(x => x.Goal.Priority).IsInEnum().WithMessage("Priority is required");
    }
}
