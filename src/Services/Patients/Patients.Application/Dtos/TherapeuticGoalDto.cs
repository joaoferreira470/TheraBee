namespace Patients.Application.Dtos;

public record TherapeuticGoalDto(
    Guid Id,
    Guid PatientId,
    Guid TherapistId,
    TherapeuticGoalType Type,
    string Description,
    TherapeuticGoalPriority Priority,
    TherapeuticGoalStatus Status,
    DateTime? CreatedAt);
