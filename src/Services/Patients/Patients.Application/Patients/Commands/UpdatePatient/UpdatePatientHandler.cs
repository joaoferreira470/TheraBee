namespace Patients.Application.Patients.Commands.UpdatePatient;

public class UpdatePatientHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdatePatientCommand, UpdatePatientResult>
{
    public async Task<UpdatePatientResult> Handle(UpdatePatientCommand command, CancellationToken cancellationToken)
    {
        var patientId = PatientId.Of(command.Patient.Id);

        var patient = await dbContext.Patients.FindAsync([patientId], cancellationToken);

        if (patient == null) 
        {
            throw new PatientNotFoundException(command.Patient.Id);
        }

        UpdatePatientWithNewValues(patient, command.Patient);

        dbContext.Patients.Update(patient);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdatePatientResult(true);
    }

    private void UpdatePatientWithNewValues(Patient patient, PatientDto patientDto)
    {
        var updatedPatientAddress = Address.Of(
                                                patientDto.PatientAddress.AddressLine,
                                                patientDto.PatientAddress.District,
                                                patientDto.PatientAddress.Location,
                                                patientDto.PatientAddress.ZipCode);
        patient.Update(
                        patientDto.Name,
                        patientDto.DateOfBirth,
                        updatedPatientAddress,
                        patientDto.Diagnosis,
                        patientDto.Info,
                        patientDto.TherapistId
                        );
    }
}
