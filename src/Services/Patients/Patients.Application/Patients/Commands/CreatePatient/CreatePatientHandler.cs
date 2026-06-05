using BuildingBlocks.CQRS;
using Patients.Application.Data;
using Patients.Application.Dtos;

namespace Patients.Application.Patients.Commands.CreatePatient;

public class CreatePatientHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : ICommandHandler<CreatePatientCommand, CreatePatientResult>
{
    public async Task<CreatePatientResult> Handle(CreatePatientCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var duplicateExists = await dbContext.Patients.AnyAsync(
            patient => patient.TherapistId == currentUserId
                && (
                    (patient.Name.ToLower() == command.Patient.Name.Trim().ToLower()
                        && patient.DateOfBirth.Date == command.Patient.DateOfBirth.Date)
                    || (!string.IsNullOrWhiteSpace(command.Patient.PhoneNumber)
                        && patient.PhoneNumber != null
                        && patient.PhoneNumber.ToLower() == command.Patient.PhoneNumber!.Trim().ToLower())
                    || (!string.IsNullOrWhiteSpace(command.Patient.Email)
                        && patient.Email != null
                        && patient.Email.ToLower() == command.Patient.Email!.Trim().ToLower())
                ),
            cancellationToken);

        if (duplicateExists)
        {
            throw new DuplicatePatientException();
        }

        var patient = CreateNewPatient(command.Patient, currentUserId);

        dbContext.Patients.Add(patient);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreatePatientResult(patient.Id.Value);
    }

    private Patient CreateNewPatient(CreatePatientDto patientDto, Guid therapistId)
    {
        var normalizedName = patientDto.Name.Trim();
        var patientAddress = Address.Of(
                                        patientDto.PatientAddress.AddressLine,
                                        patientDto.PatientAddress.District,
                                        patientDto.PatientAddress.Location,
                                        patientDto.PatientAddress.ZipCode
                                        );

        return Patient.Create(
                              PatientId.Of(Guid.NewGuid()),
                              normalizedName,
                              patientDto.DateOfBirth,
                              patientAddress,
                              patientDto.MainDiagnosis,
                              NormalizeOptional(patientDto.GeneralNotes),
                              therapistId,
                              NormalizeOptional(patientDto.Gender),
                              NormalizeOptional(patientDto.PhoneNumber),
                              NormalizeEmail(patientDto.Email),
                              NormalizeOptional(patientDto.CaregiverName),
                              NormalizeOptional(patientDto.CaregiverPhone),
                              NormalizeOptional(patientDto.ReferralReason));

    }

    private static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string? NormalizeEmail(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLowerInvariant();
}
