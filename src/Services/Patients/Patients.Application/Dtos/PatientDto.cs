using Patients.Domain.ValueObjects;

namespace Patients.Application.Dtos;

public record PatientDto(
    Guid Id,
    string Name,
    DateTime DateOfBirth,
    int CalculatedAge,
    AddressDto PatientAddress,
    string MainDiagnosis,
    string? Gender,
    string? PhoneNumber,
    string? Email,
    string? CaregiverName,
    string? CaregiverPhone,
    string? ReferralReason,
    string? GeneralNotes,
    Guid TherapistId,
    PatientStatus Status);
