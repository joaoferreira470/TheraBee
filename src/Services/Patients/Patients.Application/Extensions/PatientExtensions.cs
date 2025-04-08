namespace Patients.Application.Extensions;

public static class PatientExtensions
{
    public static IEnumerable<PatientDto> ToPatientDtoList(this IEnumerable<Patient> patients)
    {
        return patients.Select(patient => new PatientDto(
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
            ));
    }
}
