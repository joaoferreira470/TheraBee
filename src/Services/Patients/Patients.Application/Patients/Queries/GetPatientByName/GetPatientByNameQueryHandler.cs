using Patients.Application.Patients.Queries.GetPatientById;

namespace Patients.Application.Patients.Queries.GetPatientByName;

public class GetPatientByNameQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IQueryHandler<GetPatientByNameQuery, GetPatientByNameResult>
{
    public async Task<GetPatientByNameResult> Handle(GetPatientByNameQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patientName = query.Name.Trim();

        var patient = await dbContext.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == patientName && p.TherapistId == currentUserId, cancellationToken);

        if (patient == null)
        {
            throw new PatientNotFoundException(query.Name);
        }

        var patientDto = patient.ToPatientDto();

        return new GetPatientByNameResult(patientDto);
    }
}
