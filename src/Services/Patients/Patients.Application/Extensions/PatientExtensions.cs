namespace Patients.Application.Extensions;

public static class PatientExtensions
{
    public static PatientDto ToPatientDto(this Patient patient)
    {
        return new PatientDto(
            Id: patient.Id.Value,
            Name: patient.Name,
            DateOfBirth: patient.DateOfBirth,
            CalculatedAge: CalculateAge(patient.DateOfBirth),
            PatientAddress: new AddressDto(
                patient.PatientAddress.AddressLine,
                patient.PatientAddress.District,
                patient.PatientAddress.Location,
                patient.PatientAddress.ZipCode),
            MainDiagnosis: patient.MainDiagnosis,
            Gender: patient.Gender,
            PhoneNumber: patient.PhoneNumber,
            Email: patient.Email,
            CaregiverName: patient.CaregiverName,
            CaregiverPhone: patient.CaregiverPhone,
            ReferralReason: patient.ReferralReason,
            GeneralNotes: patient.GeneralNotes,
            TherapistId: patient.TherapistId,
            Status: patient.Status,
            HasPortrait: patient.PortraitStorageKey is not null,
            PortraitUpdatedAt: patient.PortraitUpdatedAt);
    }

    public static IEnumerable<PatientDto> ToPatientDtoList(this IEnumerable<Patient> patients)
    {
        return patients.Select(ToPatientDto);
    }

    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.UtcNow.Date;
        var age = today.Year - dateOfBirth.Date.Year;

        if (dateOfBirth.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}
