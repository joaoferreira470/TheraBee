
using Patients.Application.Patients.Queries.GetPatientById;

namespace Patients.Application.Patients.Queries.GetPatientByName;

public class GetPatientByNameQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetPatientByNameQuery, GetPatientByNameResult>
{
    public async Task<GetPatientByNameResult> Handle(GetPatientByNameQuery query, CancellationToken cancellationToken)
    {
        var patient = await dbContext.Patients
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(p => p.Name == query.Name, cancellationToken);

        // Se houver mais do que um paciente com o mesmo nome, esta query só trará o primeiro
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
                                          TherapistId: patient.TherapistId
                                          );

        return new GetPatientByNameResult(patientDto);
    }
}
