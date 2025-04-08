namespace Patients.Application.Patients.Queries.GetPatientById;

public class GetPatientByIdHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetPatientByIdQuery, GetPatientByIdResult>
{
    public async Task<GetPatientByIdResult> Handle(GetPatientByIdQuery query, CancellationToken cancellationToken)
    {
        var patient = await dbContext.Patients
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(p => p.Id.Value == query.Id.Value, cancellationToken);

        if (patient == null)
        {
            throw new PatientNotFoundException(query.Id.Value);
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

        return new GetPatientByIdResult(patientDto);
    }
}
