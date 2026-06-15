
namespace Patients.Application.Patients.Commands.DeletePatient;

public class DeletePatientHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IPortraitStorage portraitStorage)
    : ICommandHandler<DeletePatientCommand, DeletePatientResult>
{
    public async Task<DeletePatientResult> Handle(DeletePatientCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patientId = PatientId.Of(command.PatientId);

        var patient = await dbContext.Patients
            .FirstOrDefaultAsync(p => p.Id == patientId && p.TherapistId == currentUserId, cancellationToken);

        if (patient == null)
        {
            throw new PatientNotFoundException(command.PatientId);
        }

        var sessionIds = await dbContext.Sessions
            .Where(session => session.PatientId == command.PatientId && session.TherapistId == currentUserId)
            .Select(session => session.Id)
            .ToListAsync(cancellationToken);

        var goalIds = await dbContext.TherapeuticGoals
            .Where(goal => goal.PatientId == command.PatientId && goal.TherapistId == currentUserId)
            .Select(goal => goal.Id)
            .ToListAsync(cancellationToken);

        var assessments = await dbContext.SessionGoalAssessments
            .Where(assessment =>
                assessment.TherapistId == currentUserId &&
                (sessionIds.Contains(assessment.SessionId) || goalIds.Contains(assessment.TherapeuticGoalId)))
            .ToListAsync(cancellationToken);

        var sessionGoals = await dbContext.SessionGoals
            .Where(sessionGoal =>
                sessionGoal.TherapistId == currentUserId &&
                (sessionIds.Contains(sessionGoal.SessionId) || goalIds.Contains(sessionGoal.TherapeuticGoalId)))
            .ToListAsync(cancellationToken);

        var reports = await dbContext.Reports
            .Where(report => report.PatientId == command.PatientId && report.TherapistId == currentUserId)
            .ToListAsync(cancellationToken);

        var sessions = await dbContext.Sessions
            .Where(session => sessionIds.Contains(session.Id))
            .ToListAsync(cancellationToken);

        var goals = await dbContext.TherapeuticGoals
            .Where(goal => goalIds.Contains(goal.Id))
            .ToListAsync(cancellationToken);

        dbContext.SessionGoalAssessments.RemoveRange(assessments);
        dbContext.SessionGoals.RemoveRange(sessionGoals);
        dbContext.Reports.RemoveRange(reports);
        dbContext.Sessions.RemoveRange(sessions);
        dbContext.TherapeuticGoals.RemoveRange(goals);
        dbContext.Patients.Remove(patient);

        await dbContext.SaveChangesAsync(cancellationToken);

        if (patient.PortraitStorageKey is not null)
        {
            await portraitStorage.DeleteAsync(patient.PortraitStorageKey, cancellationToken);
        }

        return new DeletePatientResult(true);
    }
}
