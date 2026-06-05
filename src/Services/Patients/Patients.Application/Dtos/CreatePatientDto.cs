namespace Patients.Application.Dtos;

public record CreatePatientDto(
    string Name,
    DateTime DateOfBirth,
    AddressDto PatientAddress,
    string MainDiagnosis,
    string? Gender,
    string? PhoneNumber,
    string? Email,
    string? CaregiverName,
    string? CaregiverPhone,
    string? ReferralReason,
    string? GeneralNotes);
