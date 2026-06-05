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

        var patientDto = new PatientDto(
            Id: patient.Id.Value,
            Name: patient.Name,
            DateOfBirth: patient.DateOfBirth,
            PatientAddress: new AddressDto(
                patient.PatientAddress.AddressLine,
                patient.PatientAddress.District,
                patient.PatientAddress.Location,
                patient.PatientAddress.ZipCode),
            Diagnosis: patient.Diagnosis,
            Info: patient.Info,
            TherapistId: patient.TherapistId,
            Status: patient.Status);

        return new GetPatientByNameResult(patientDto);
    }
}
