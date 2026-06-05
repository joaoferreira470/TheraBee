namespace Patients.Application.Sessions.Commands.AddSessionGoals;

public class AddSessionGoalsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<AddSessionGoalsCommand, AddSessionGoalsResult>
{
    public async Task<AddSessionGoalsResult> Handle(AddSessionGoalsCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await dbContext.Sessions
            .Include(session => session.SessionGoals)
            .FirstOrDefaultAsync(session => session.Id == command.SessionId && session.TherapistId == currentUserId, cancellationToken);

        if (session == null)
        {
            throw new SessionNotFoundException(command.SessionId);
        }

        var goalIds = command.Goals.GoalIds
            .Where(goalId => goalId != Guid.Empty)
            .Distinct()
            .ToArray();

        var goals = await dbContext.TherapeuticGoals
            .Where(goal => goal.TherapistId == currentUserId
                && goal.PatientId == session.PatientId
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
            session.AddGoal(SessionGoal.Create(Guid.NewGuid(), session.Id, goalId, currentUserId));
        }

        dbContext.Sessions.Update(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddSessionGoalsResult(true);
    }
}
