namespace Patients.Application.Dtos;

public record CreatePatientDto(
    string Name,
    DateTime DateOfBirth,
    AddressDto PatientAddress,
    string Diagnosis,
    string Info,
    Guid TherapistId);
