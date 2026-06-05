namespace Patients.Application.Sessions.Commands.CreateSession;

public class CreateSessionHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<CreateSessionCommand, CreateSessionResult>
{
    public async Task<CreateSessionResult> Handle(CreateSessionCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patientExists = await dbContext.Patients.AnyAsync(
            patient => patient.Id == PatientId.Of(command.PatientId) && patient.TherapistId == currentUserId,
            cancellationToken);

        if (!patientExists)
        {
            throw new PatientNotFoundException(command.PatientId);
        }

        var sessionId = Guid.NewGuid();
        var session = Session.Create(
            id: sessionId,
            patientId: command.PatientId,
            therapistId: currentUserId,
            startDateTime: command.Session.StartDateTime,
            endDateTime: command.Session.EndDateTime,
            type: command.Session.Type,
            location: command.Session.Location);

        var goalIds = command.Session.GoalIds?
            .Where(goalId => goalId != Guid.Empty)
            .Distinct()
            .ToArray() ?? Array.Empty<Guid>();

        if (goalIds.Length > 0)
        {
            var expectedGoalType = command.Session.Type == SessionType.Intervention
                ? TherapeuticGoalType.Objective
                : TherapeuticGoalType.Area;

            var goals = await dbContext.TherapeuticGoals
                .Where(goal => goal.TherapistId == currentUserId
                    && goal.PatientId == command.PatientId
                    && goal.Type == expectedGoalType
                    && goalIds.Contains(goal.Id))
                .Select(goal => goal.Id)
                .ToListAsync(cancellationToken);

            if (goals.Count != goalIds.Length)
            {
                var missingGoalId = goalIds.Except(goals).FirstOrDefault();
                throw new TherapeuticGoalNotFoundException(missingGoalId == Guid.Empty ? goalIds[0] : missingGoalId);
            }

            foreach (var goalId in goals)
            {
                session.AddGoal(SessionGoal.Create(Guid.NewGuid(), sessionId, goalId, currentUserId));
            }
        }

        dbContext.Sessions.Add(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateSessionResult(session.Id);
    }
}
