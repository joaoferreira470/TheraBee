namespace Patients.Application.Sessions.Queries.GetSessionGoalAssessments;

public record GetSessionGoalAssessmentsQuery(Guid SessionId)
    : IQuery<GetSessionGoalAssessmentsResult>;

public record GetSessionGoalAssessmentsResult(IEnumerable<SessionGoalAssessmentDto> Assessments);
