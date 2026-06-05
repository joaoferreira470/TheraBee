namespace Patients.Application.Sessions.Commands.SetSessionGoals;

public class SetSessionGoalsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<SetSessionGoalsCommand, SetSessionGoalsResult>
{
    public async Task<SetSessionGoalsResult> Handle(SetSessionGoalsCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await dbContext.Sessions
            .Include(item => item.SessionGoals)
            .FirstOrDefaultAsync(item => item.Id == command.SessionId && item.TherapistId == currentUserId, cancellationToken);

        if (session == null)
        {
            throw new SessionNotFoundException(command.SessionId);
        }

        var goalIds = command.Goals.GoalIds
            .Where(goalId => goalId != Guid.Empty)
            .Distinct()
            .ToArray();

        var expectedGoalType = session.Type == SessionType.Intervention
            ? TherapeuticGoalType.ShortTerm
            : TherapeuticGoalType.LongTerm;

        var goals = await dbContext.TherapeuticGoals
            .Where(goal => goal.TherapistId == currentUserId
                && goal.PatientId == session.PatientId
                && goal.Type == expectedGoalType
                && goalIds.Contains(goal.Id))
            .ToListAsync(cancellationToken);

        if (goals.Count != goalIds.Length)
        {
            var missingGoalId = goalIds.Except(goals.Select(goal => goal.Id)).FirstOrDefault();
            throw new TherapeuticGoalNotFoundException(missingGoalId == Guid.Empty ? goalIds[0] : missingGoalId);
        }

        var existingGoalIds = session.SessionGoals.Select(goal => goal.TherapeuticGoalId).ToHashSet();
        var selectedGoalIds = goals.Select(goal => goal.Id).ToHashSet();

        var goalsToRemove = session.SessionGoals
            .Where(goal => !selectedGoalIds.Contains(goal.TherapeuticGoalId))
            .ToList();

        if (goalsToRemove.Count > 0)
        {
            dbContext.SessionGoals.RemoveRange(goalsToRemove);
        }

        foreach (var goalId in selectedGoalIds.Except(existingGoalIds))
        {
            session.AddGoal(SessionGoal.Create(Guid.NewGuid(), session.Id, goalId, currentUserId));
        }

        dbContext.Sessions.Update(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new SetSessionGoalsResult(true);
    }
}
