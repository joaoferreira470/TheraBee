namespace Patients.Application.Dtos;

public record UpdatePatientDto(
    Guid Id,
    string Name,
    DateTime DateOfBirth,
    AddressDto PatientAddress,
    string Diagnosis,
    string Info);
