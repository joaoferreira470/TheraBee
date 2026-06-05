namespace Patients.Application.Sessions.Commands.CompleteSession;

public class CompleteSessionHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<CompleteSessionCommand, CompleteSessionResult>
{
    public async Task<CompleteSessionResult> Handle(CompleteSessionCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await dbContext.Sessions
            .FirstOrDefaultAsync(session => session.Id == command.SessionId && session.TherapistId == currentUserId, cancellationToken);

        if (session == null)
        {
            throw new SessionNotFoundException(command.SessionId);
        }

        session.Complete(
            command.Session.ClinicalSummary,
            command.Session.ObjectivesWorked,
            command.Session.ProgressRating,
            command.Session.Activities,
            command.Session.PatientResponse,
            command.Session.Difficulties,
            command.Session.Recommendations,
            command.Session.NextSteps);

        dbContext.Sessions.Update(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CompleteSessionResult(true);
    }
}
