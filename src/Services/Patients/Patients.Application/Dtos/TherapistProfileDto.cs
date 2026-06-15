namespace Patients.Application.Dtos;

public record TherapistProfileDto(
    Guid Id,
    Guid UserId,
    string ProfessionalName,
    string Profession,
    string? Specialties,
    string? ProfessionalNumber,
    string? PhoneNumber,
    string? Workplace,
    string? ReportSignature,
    bool HasPortrait,
    DateTime? PortraitUpdatedAt);

