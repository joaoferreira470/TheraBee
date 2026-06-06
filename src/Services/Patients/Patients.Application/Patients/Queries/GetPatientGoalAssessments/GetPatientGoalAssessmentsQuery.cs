namespace Patients.Application.Patients.Queries.GetPatientGoalAssessments;

public record GetPatientGoalAssessmentsQuery(Guid PatientId)
    : IQuery<GetPatientGoalAssessmentsResult>;

public record GetPatientGoalAssessmentsResult(IEnumerable<SessionGoalAssessmentDto> Assessments);
