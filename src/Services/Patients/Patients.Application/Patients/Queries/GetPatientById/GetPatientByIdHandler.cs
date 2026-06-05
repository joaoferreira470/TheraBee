namespace Patients.Application.Patients.Queries.GetPatientById;

public class GetPatientByIdHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetPatientByIdQuery, GetPatientByIdResult>
{
    public async Task<GetPatientByIdResult> Handle(GetPatientByIdQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patient = await dbContext.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == query.Id && p.TherapistId == currentUserId, cancellationToken);

        if (patient == null)
        {
            throw new PatientNotFoundException(query.Id.Value);
        }

        var patientDto = patient.ToPatientDto();

        return new GetPatientByIdResult(patientDto);
    }
}
