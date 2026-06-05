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
            description: command.Goal.Description.Trim(),
            area: command.Goal.Area.Trim(),
            priority: command.Goal.Priority,
            reviewDate: command.Goal.ReviewDate);

        dbContext.TherapeuticGoals.Add(goal);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateTherapeuticGoalResult(goal.Id);
    }
}
