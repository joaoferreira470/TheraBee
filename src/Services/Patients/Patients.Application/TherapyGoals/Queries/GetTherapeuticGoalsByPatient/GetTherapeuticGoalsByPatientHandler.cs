namespace Patients.Application.TherapyGoals.Queries.GetTherapeuticGoalsByPatient;

public class GetTherapeuticGoalsByPatientHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetTherapeuticGoalsByPatientQuery, GetTherapeuticGoalsByPatientResult>
{
    public async Task<GetTherapeuticGoalsByPatientResult> Handle(GetTherapeuticGoalsByPatientQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patientExists = await dbContext.Patients.AnyAsync(
            patient => patient.Id == PatientId.Of(query.PatientId) && patient.TherapistId == currentUserId,
            cancellationToken);

        if (!patientExists)
        {
            throw new PatientNotFoundException(query.PatientId);
        }

        var goals = await dbContext.TherapeuticGoals
            .AsNoTracking()
            .Where(goal => goal.PatientId == query.PatientId && goal.TherapistId == currentUserId)
            .OrderBy(goal => goal.Area)
            .ThenBy(goal => goal.Description)
            .ToListAsync(cancellationToken);

        return new GetTherapeuticGoalsByPatientResult(goals.ToTherapeuticGoalDtoList());
    }
}
