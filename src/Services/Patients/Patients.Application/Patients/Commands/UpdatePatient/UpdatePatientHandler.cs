namespace Patients.Application.Patients.Commands.UpdatePatient;

public class UpdatePatientHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<UpdatePatientCommand, UpdatePatientResult>
{
    public async Task<UpdatePatientResult> Handle(UpdatePatientCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var patientId = PatientId.Of(command.Patient.Id);

        var patient = await dbContext.Patients
            .FirstOrDefaultAsync(p => p.Id == patientId && p.TherapistId == currentUserId, cancellationToken);

        if (patient == null) 
        {
            throw new PatientNotFoundException(command.Patient.Id);
        }

        UpdatePatientWithNewValues(patient, command.Patient);

        dbContext.Patients.Update(patient);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdatePatientResult(true);
    }

    private void UpdatePatientWithNewValues(Patient patient, UpdatePatientDto patientDto)
    {
        var updatedPatientAddress = Address.Of(
                                                patientDto.PatientAddress.AddressLine,
                                                patientDto.PatientAddress.District,
                                                patientDto.PatientAddress.Location,
                                                patientDto.PatientAddress.ZipCode);
        patient.Update(
                        patientDto.Name.Trim(),
                        patientDto.DateOfBirth,
                        updatedPatientAddress,
                        patientDto.MainDiagnosis,
                        NormalizeOptional(patientDto.GeneralNotes),
                        patient.TherapistId,
                        NormalizeOptional(patientDto.Gender),
                        NormalizeOptional(patientDto.PhoneNumber),
                        NormalizeEmail(patientDto.Email),
                        NormalizeOptional(patientDto.CaregiverName),
                        NormalizeOptional(patientDto.CaregiverPhone),
                        NormalizeOptional(patientDto.ReferralReason)
                        );
    }

    private static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string? NormalizeEmail(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLowerInvariant();
}
