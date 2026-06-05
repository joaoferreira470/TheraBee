namespace Patients.Application.Dtos;

public record TherapeuticGoalDto(
    Guid Id,
    Guid PatientId,
    Guid TherapistId,
    string Description,
    string Area,
    TherapeuticGoalPriority Priority,
    TherapeuticGoalStatus Status,
    DateTime? ReviewDate,
    DateTime? CreatedAt);
