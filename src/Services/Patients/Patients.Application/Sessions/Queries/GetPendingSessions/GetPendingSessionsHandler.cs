namespace Patients.Application.Sessions.Queries.GetPendingSessions;

public class GetPendingSessionsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetPendingSessionsQuery, GetPendingSessionsResult>
{
    public async Task<GetPendingSessionsResult> Handle(GetPendingSessionsQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var now = DateTime.UtcNow;

        var sessions = await dbContext.Sessions
            .AsNoTracking()
            .Include(session => session.SessionGoals)
            .Include(session => session.SessionGoalAssessments)
            .Where(session => session.TherapistId == currentUserId
                && (session.Status == SessionStatus.Scheduled || session.Status == SessionStatus.Rescheduled)
                && session.EndDateTime <= now)
            .OrderBy(session => session.StartDateTime)
            .ToListAsync(cancellationToken);

        return new GetPendingSessionsResult(sessions.ToSessionDtoList());
    }
}
