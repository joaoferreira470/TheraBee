namespace Patients.Application.Sessions.Commands.RescheduleSession;

public class RescheduleSessionHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<RescheduleSessionCommand, RescheduleSessionResult>
{
    public async Task<RescheduleSessionResult> Handle(RescheduleSessionCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await dbContext.Sessions
            .FirstOrDefaultAsync(session => session.Id == command.SessionId && session.TherapistId == currentUserId, cancellationToken);

        if (session == null)
        {
            throw new SessionNotFoundException(command.SessionId);
        }

        session.Reschedule(
            command.Session.StartDateTime,
            command.Session.EndDateTime,
            command.Session.Type,
            command.Session.Location);

        dbContext.Sessions.Update(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RescheduleSessionResult(true);
    }
}
