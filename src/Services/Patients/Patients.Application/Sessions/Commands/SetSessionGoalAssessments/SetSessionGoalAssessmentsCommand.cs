using FluentValidation;

namespace Patients.Application.Sessions.Commands.SetSessionGoalAssessments;

public record SetSessionGoalAssessmentsCommand(Guid SessionId, SetSessionGoalAssessmentsDto Assessments)
    : ICommand<SetSessionGoalAssessmentsResult>;

public record SetSessionGoalAssessmentsResult(bool IsSuccess);

public class SetSessionGoalAssessmentsValidator : AbstractValidator<SetSessionGoalAssessmentsCommand>
{
    public SetSessionGoalAssessmentsValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty().WithMessage("SessionId is required");
        RuleFor(x => x.Assessments).NotNull().WithMessage("Assessments are required");
        RuleForEach(x => x.Assessments.Assessments).ChildRules(assessment =>
        {
            assessment.RuleFor(item => item.TherapeuticGoalId)
                .NotEmpty()
                .WithMessage("TherapeuticGoalId is required");
            assessment.RuleFor(item => item.Score)
                .InclusiveBetween(0, 10)
                .WithMessage("Score must be between 0 and 10");
        });
        RuleFor(x => x.Assessments.Assessments)
            .Must(assessments => assessments == null || assessments.Select(item => item.TherapeuticGoalId).Distinct().Count() == assessments.Count())
            .WithMessage("TherapeuticGoalId values must be unique");
    }
}
