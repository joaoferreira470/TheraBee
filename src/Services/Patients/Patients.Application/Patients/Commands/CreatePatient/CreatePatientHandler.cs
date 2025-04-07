using BuildingBlocks.CQRS;
using Patients.Application.Data;
using Patients.Application.Dtos;

namespace Patients.Application.Patients.Commands.CreatePatient;

public class CreatePatientHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreatePatientCommand, CreatePatientResult>
{
    public async Task<CreatePatientResult> Handle(CreatePatientCommand command, CancellationToken cancellationToken)
    {
        var patient = CreateNewPatient(command.Patient);

        dbContext.Patients.Add(patient);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreatePatientResult(patient.Id.Value);
    }

    private Patient CreateNewPatient(PatientDto patientDto)
    {
        var patientAddress = Address.Of(
                                        patientDto.PatientAddress.AddressLine,
                                        patientDto.PatientAddress.District,
                                        patientDto.PatientAddress.Location,
                                        patientDto.PatientAddress.ZipCode
                                        );
        
        //ver aula 236, caso dê pau : Parâmetros do Create estão passados de maneira diferente
        return Patient.Create(
                              PatientId.Of(patientDto.Id),
                              patientDto.Name,
                              patientDto.DateOfBirth,
                              patientAddress,
                              patientDto.Diagnosis,
                              patientDto.Info,
                              patientDto.TherapistId);

    }
}
