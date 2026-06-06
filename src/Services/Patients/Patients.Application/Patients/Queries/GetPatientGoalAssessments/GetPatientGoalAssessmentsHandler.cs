namespace Patients.Application.Patients.Queries.GetPatientGoalAssessments;

public class GetPatientGoalAssessmentsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetPatientGoalAssessmentsQuery, GetPatientGoalAssessmentsResult>
{
    public async Task<GetPatientGoalAssessmentsResult> Handle(GetPatientGoalAssessmentsQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var sessions = await dbContext.Sessions
            .AsNoTracking()
            .Where(session => session.TherapistId == currentUserId && session.PatientId == query.PatientId)
            .Include(session => session.SessionGoalAssessments)
            .OrderByDescending(session => session.StartDateTime)
            .ToListAsync(cancellationToken);

        var assessments = sessions
            .SelectMany(session => session.SessionGoalAssessments.Select(assessment => new SessionGoalAssessmentDto(
                assessment.Id,
                assessment.SessionId,
                assessment.TherapeuticGoalId,
                assessment.Score,
                assessment.ClinicalNotes,
                session.StartDateTime,
                assessment.CreatedAt)))
            .OrderByDescending(assessment => assessment.SessionStartDateTime)
            .ThenByDescending(assessment => assessment.CreatedAt)
            .ToList();

        return new GetPatientGoalAssessmentsResult(assessments);
    }
}
