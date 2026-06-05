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

        var hasSchedulingConflict = await dbContext.Sessions
            .AsNoTracking()
            .AnyAsync(
                otherSession => otherSession.TherapistId == currentUserId
                    && otherSession.Id != command.SessionId
                    && (otherSession.Status == SessionStatus.Scheduled || otherSession.Status == SessionStatus.Rescheduled)
                    && otherSession.StartDateTime < command.Session.EndDateTime
                    && otherSession.EndDateTime > command.Session.StartDateTime,
                cancellationToken);

        if (hasSchedulingConflict)
        {
            throw new SessionSchedulingConflictException(command.Session.StartDateTime, command.Session.EndDateTime);
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
