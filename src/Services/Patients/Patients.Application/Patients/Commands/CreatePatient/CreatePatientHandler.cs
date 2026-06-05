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
                && patient.Name.ToLower() == command.Patient.Name.Trim().ToLower()
                && patient.DateOfBirth.Date == command.Patient.DateOfBirth.Date,
            cancellationToken);

        if (duplicateExists)
        {
            throw new DuplicatePatientException(command.Patient.Name.Trim(), command.Patient.DateOfBirth);
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
                              patientDto.Diagnosis,
                              patientDto.Info,
                              therapistId);

    }
}
