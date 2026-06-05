namespace Patients.Application.Sessions.Queries.GetSessions;

public class GetSessionsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetSessionsQuery, GetSessionsResult>
{
    public async Task<GetSessionsResult> Handle(GetSessionsQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var sessions = await dbContext.Sessions
            .AsNoTracking()
            .Include(session => session.SessionGoals)
            .Where(session => session.TherapistId == currentUserId)
            .OrderByDescending(session => session.StartDateTime)
            .ToListAsync(cancellationToken);

        return new GetSessionsResult(sessions.ToSessionDtoList());
    }
}
