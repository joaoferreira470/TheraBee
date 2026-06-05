using FluentValidation;

namespace Patients.Application.TherapyGoals.Commands.CreateTherapeuticGoal;

public record CreateTherapeuticGoalCommand(Guid PatientId, CreateTherapeuticGoalDto Goal)
    : ICommand<CreateTherapeuticGoalResult>;

public record CreateTherapeuticGoalResult(Guid Id);

public class CreateTherapeuticGoalValidator : AbstractValidator<CreateTherapeuticGoalCommand>
{
    public CreateTherapeuticGoalValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("PatientId is required");
        RuleFor(x => x.Goal.Type).IsInEnum().WithMessage("Type is required");
        RuleFor(x => x.Goal.Description).NotEmpty().MaximumLength(500).WithMessage("Description is required");
        RuleFor(x => x.Goal.Area).NotEmpty().MaximumLength(150).WithMessage("Area is required");
        RuleFor(x => x.Goal.Priority).IsInEnum().WithMessage("Priority is required");
    }
}
