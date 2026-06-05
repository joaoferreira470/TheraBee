namespace Patients.Application.TherapyGoals.Commands.UpdateTherapeuticGoal;

public class UpdateTherapeuticGoalHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<UpdateTherapeuticGoalCommand, UpdateTherapeuticGoalResult>
{
    public async Task<UpdateTherapeuticGoalResult> Handle(UpdateTherapeuticGoalCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var goal = await dbContext.TherapeuticGoals
            .FirstOrDefaultAsync(g => g.Id == command.GoalId && g.TherapistId == currentUserId, cancellationToken);

        if (goal == null)
        {
            throw new TherapeuticGoalNotFoundException(command.GoalId);
        }

        goal.Update(
            description: command.Goal.Description.Trim(),
            area: command.Goal.Area.Trim(),
            priority: command.Goal.Priority,
            reviewDate: command.Goal.ReviewDate);

        dbContext.TherapeuticGoals.Update(goal);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateTherapeuticGoalResult(true);
    }
}
