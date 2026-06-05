namespace Patients.Application.TherapyGoals.Commands.UpdateTherapeuticGoalStatus;

public class UpdateTherapeuticGoalStatusHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<UpdateTherapeuticGoalStatusCommand, UpdateTherapeuticGoalStatusResult>
{
    public async Task<UpdateTherapeuticGoalStatusResult> Handle(UpdateTherapeuticGoalStatusCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var goal = await dbContext.TherapeuticGoals
            .FirstOrDefaultAsync(g => g.Id == command.GoalId && g.TherapistId == currentUserId, cancellationToken);

        if (goal == null)
        {
            throw new TherapeuticGoalNotFoundException(command.GoalId);
        }

        goal.UpdateStatus(command.Status);
        dbContext.TherapeuticGoals.Update(goal);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateTherapeuticGoalStatusResult(true);
    }
}
