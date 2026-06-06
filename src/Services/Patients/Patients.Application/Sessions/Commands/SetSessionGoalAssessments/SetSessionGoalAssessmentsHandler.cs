namespace Patients.Application.Sessions.Commands.SetSessionGoalAssessments;

public class SetSessionGoalAssessmentsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<SetSessionGoalAssessmentsCommand, SetSessionGoalAssessmentsResult>
{
    public async Task<SetSessionGoalAssessmentsResult> Handle(SetSessionGoalAssessmentsCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await dbContext.Sessions
            .Include(item => item.SessionGoals)
            .Include(item => item.SessionGoalAssessments)
            .FirstOrDefaultAsync(item => item.Id == command.SessionId && item.TherapistId == currentUserId, cancellationToken);

        if (session == null)
        {
            throw new SessionNotFoundException(command.SessionId);
        }

        if (session.Status is SessionStatus.Completed or SessionStatus.Cancelled)
        {
            throw new InvalidOperationException("Completed or cancelled sessions cannot be updated.");
        }

        var assessments = command.Assessments.Assessments
            .Where(item => item.TherapeuticGoalId != Guid.Empty)
            .DistinctBy(item => item.TherapeuticGoalId)
            .ToArray();

        var sessionGoalIds = session.SessionGoals
            .Select(item => item.TherapeuticGoalId)
            .ToHashSet();

        if (sessionGoalIds.Count == 0)
        {
            if (assessments.Length > 0)
            {
                throw new InvalidOperationException("This session does not have goals attached to assess.");
            }
        }
        else if (assessments.Length != sessionGoalIds.Count)
        {
            throw new InvalidOperationException("Each attached goal must have one assessment before confirming the session.");
        }

        var invalidGoalId = assessments
            .Select(item => item.TherapeuticGoalId)
            .FirstOrDefault(goalId => !sessionGoalIds.Contains(goalId));

        if (invalidGoalId != Guid.Empty)
        {
            throw new TherapeuticGoalNotFoundException(invalidGoalId);
        }

        var existingAssessments = session.SessionGoalAssessments
            .ToDictionary(item => item.TherapeuticGoalId, item => item);

        var submittedGoalIds = assessments.Select(item => item.TherapeuticGoalId).ToHashSet();

        var assessmentsToRemove = session.SessionGoalAssessments
            .Where(item => !submittedGoalIds.Contains(item.TherapeuticGoalId))
            .ToList();

        if (assessmentsToRemove.Count > 0)
        {
            dbContext.SessionGoalAssessments.RemoveRange(assessmentsToRemove);
        }

        foreach (var assessment in assessments)
        {
            if (existingAssessments.TryGetValue(assessment.TherapeuticGoalId, out var existingAssessment))
            {
                existingAssessment.Update(assessment.Score, assessment.ClinicalNotes);
                continue;
            }

            var newAssessment = SessionGoalAssessment.Create(
                Guid.NewGuid(),
                session.Id,
                assessment.TherapeuticGoalId,
                currentUserId,
                assessment.Score,
                assessment.ClinicalNotes);

            dbContext.SessionGoalAssessments.Add(newAssessment);
            session.SessionGoalAssessments.Add(newAssessment);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new SetSessionGoalAssessmentsResult(true);
    }
}
