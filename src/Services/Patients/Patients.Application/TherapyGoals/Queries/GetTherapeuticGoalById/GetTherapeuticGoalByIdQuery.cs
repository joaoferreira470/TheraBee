namespace Patients.Application.TherapyGoals.Queries.GetTherapeuticGoalById;

public record GetTherapeuticGoalByIdQuery(Guid GoalId)
    : IQuery<GetTherapeuticGoalByIdResult>;

public record GetTherapeuticGoalByIdResult(TherapeuticGoalDto TherapeuticGoal);
