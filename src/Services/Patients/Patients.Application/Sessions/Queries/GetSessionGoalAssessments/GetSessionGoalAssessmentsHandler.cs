namespace Patients.Application.Sessions.Queries.GetSessionGoalAssessments;

public class GetSessionGoalAssessmentsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetSessionGoalAssessmentsQuery, GetSessionGoalAssessmentsResult>
{
    public async Task<GetSessionGoalAssessmentsResult> Handle(GetSessionGoalAssessmentsQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await dbContext.Sessions
            .AsNoTracking()
            .Include(item => item.SessionGoalAssessments)
            .FirstOrDefaultAsync(item => item.Id == query.SessionId && item.TherapistId == currentUserId, cancellationToken);

        if (session == null)
        {
            throw new SessionNotFoundException(query.SessionId);
        }

        return new GetSessionGoalAssessmentsResult(session.SessionGoalAssessments.Select(assessment => new SessionGoalAssessmentDto(
            assessment.Id,
            assessment.SessionId,
            assessment.TherapeuticGoalId,
            assessment.Score,
            assessment.ClinicalNotes,
            session.StartDateTime,
            assessment.CreatedAt)));
    }
}
