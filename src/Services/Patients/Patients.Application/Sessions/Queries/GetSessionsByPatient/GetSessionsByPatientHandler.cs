namespace Patients.Application.Sessions.Queries.GetSessionsByPatient;

public class GetSessionsByPatientHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetSessionsByPatientQuery, GetSessionsByPatientResult>
{
    public async Task<GetSessionsByPatientResult> Handle(GetSessionsByPatientQuery query, CancellationToken cancellationToken)
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

        var sessions = await dbContext.Sessions
            .AsNoTracking()
            .Include(session => session.SessionGoals)
            .Where(session => session.PatientId == query.PatientId && session.TherapistId == currentUserId)
            .OrderByDescending(session => session.StartDateTime)
            .ToListAsync(cancellationToken);

        return new GetSessionsByPatientResult(sessions.ToSessionDtoList());
    }
}
