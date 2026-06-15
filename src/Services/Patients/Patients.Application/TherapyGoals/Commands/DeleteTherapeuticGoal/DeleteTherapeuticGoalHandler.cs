namespace Patients.Application.TherapyGoals.Commands.DeleteTherapeuticGoal;

public class DeleteTherapeuticGoalHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<DeleteTherapeuticGoalCommand, DeleteTherapeuticGoalResult>
{
    public async Task<DeleteTherapeuticGoalResult> Handle(
        DeleteTherapeuticGoalCommand command,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var goal = await dbContext.TherapeuticGoals
            .FirstOrDefaultAsync(
                item => item.Id == command.GoalId && item.TherapistId == currentUserId,
                cancellationToken);

        if (goal == null)
        {
            throw new TherapeuticGoalNotFoundException(command.GoalId);
        }

        var assessments = await dbContext.SessionGoalAssessments
            .Where(item =>
                item.TherapeuticGoalId == command.GoalId &&
                item.TherapistId == currentUserId)
            .ToListAsync(cancellationToken);

        var sessionGoals = await dbContext.SessionGoals
            .Where(item =>
                item.TherapeuticGoalId == command.GoalId &&
                item.TherapistId == currentUserId)
            .ToListAsync(cancellationToken);

        dbContext.SessionGoalAssessments.RemoveRange(assessments);
        dbContext.SessionGoals.RemoveRange(sessionGoals);
        dbContext.TherapeuticGoals.Remove(goal);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteTherapeuticGoalResult(true);
    }
}
