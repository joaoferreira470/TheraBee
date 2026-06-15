using FluentValidation;

namespace Patients.Application.TherapyGoals.Commands.DeleteTherapeuticGoal;

public record DeleteTherapeuticGoalCommand(Guid GoalId)
    : ICommand<DeleteTherapeuticGoalResult>;

public record DeleteTherapeuticGoalResult(bool IsSuccess);

public class DeleteTherapeuticGoalCommandValidator : AbstractValidator<DeleteTherapeuticGoalCommand>
{
    public DeleteTherapeuticGoalCommandValidator()
    {
        RuleFor(x => x.GoalId).NotEmpty().WithMessage("GoalId is required");
    }
}
