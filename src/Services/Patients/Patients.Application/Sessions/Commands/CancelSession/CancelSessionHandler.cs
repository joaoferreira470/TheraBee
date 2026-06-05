namespace Patients.Application.Sessions.Commands.CancelSession;

public class CancelSessionHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<CancelSessionCommand, CancelSessionResult>
{
    public async Task<CancelSessionResult> Handle(CancelSessionCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await dbContext.Sessions
            .FirstOrDefaultAsync(session => session.Id == command.SessionId && session.TherapistId == currentUserId, cancellationToken);

        if (session == null)
        {
            throw new SessionNotFoundException(command.SessionId);
        }

        session.Cancel(command.Session.CancellationReason);

        dbContext.Sessions.Update(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CancelSessionResult(true);
    }
}
