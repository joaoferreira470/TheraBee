using Patients.Domain.ValueObjects;

namespace Patients.Application.Dtos;

public record PatientDto(
    Guid Id,
    string Name,
    DateTime DateOfBirth,
    AddressDto PatientAddress,
    string Diagnosis,
    string Info,
    Guid TherapistId);
