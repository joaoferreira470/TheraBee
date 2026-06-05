namespace Patients.Application.Sessions.Queries.GetSessionById;

public class GetSessionByIdHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetSessionByIdQuery, GetSessionByIdResult>
{
    public async Task<GetSessionByIdResult> Handle(GetSessionByIdQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await dbContext.Sessions
            .AsNoTracking()
            .Include(session => session.SessionGoals)
            .FirstOrDefaultAsync(session => session.Id == query.SessionId && session.TherapistId == currentUserId, cancellationToken);

        if (session == null)
        {
            throw new SessionNotFoundException(query.SessionId);
        }

        return new GetSessionByIdResult(session.ToSessionDto());
    }
}
