namespace Patients.Application.TherapyGoals.Commands.CreateTherapeuticGoal;

public class CreateTherapeuticGoalHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<CreateTherapeuticGoalCommand, CreateTherapeuticGoalResult>
{
    public async Task<CreateTherapeuticGoalResult> Handle(CreateTherapeuticGoalCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patientId = PatientId.Of(command.PatientId);

        var patientExists = await dbContext.Patients.AnyAsync(
            patient => patient.Id == patientId && patient.TherapistId == currentUserId,
            cancellationToken);

        if (!patientExists)
        {
            throw new PatientNotFoundException(command.PatientId);
        }

        var goal = TherapeuticGoal.Create(
            id: Guid.NewGuid(),
            patientId: command.PatientId,
            therapistId: currentUserId,
            type: command.Goal.Type,
            parentGoalId: command.Goal.ParentGoalId,
            description: command.Goal.Description.Trim(),
            area: command.Goal.Area.Trim(),
            priority: command.Goal.Priority,
            reviewDate: command.Goal.ReviewDate);

        if (command.Goal.ParentGoalId is Guid parentGoalId)
        {
            var parentGoal = await dbContext.TherapeuticGoals
                .FirstOrDefaultAsync(goal => goal.Id == parentGoalId
                    && goal.PatientId == command.PatientId
                    && goal.TherapistId == currentUserId,
                    cancellationToken);

            if (parentGoal == null || parentGoal.Type != TherapeuticGoalType.LongTerm)
            {
                throw new InvalidOperationException("Short-term goals must reference an existing long-term goal for the same patient.");
            }
        }

        dbContext.TherapeuticGoals.Add(goal);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateTherapeuticGoalResult(goal.Id);
    }
}
