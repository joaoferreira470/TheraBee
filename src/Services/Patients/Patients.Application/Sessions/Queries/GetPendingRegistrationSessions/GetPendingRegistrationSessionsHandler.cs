namespace Patients.Application.Sessions.Queries.GetPendingRegistrationSessions;

public class GetPendingRegistrationSessionsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetPendingRegistrationSessionsQuery, GetPendingRegistrationSessionsResult>
{
    public async Task<GetPendingRegistrationSessionsResult> Handle(GetPendingRegistrationSessionsQuery query, CancellationToken cancellationToken)
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
            .OrderBy(session => session.EndDateTime)
            .ToListAsync(cancellationToken);

        return new GetPendingRegistrationSessionsResult(sessions.ToSessionDtoList());
    }
}
