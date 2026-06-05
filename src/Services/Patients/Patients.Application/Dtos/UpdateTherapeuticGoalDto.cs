namespace Patients.Application.Dtos;

public record UpdateTherapeuticGoalDto(
    TherapeuticGoalType Type,
    Guid? ParentGoalId,
    string Description,
    string Area,
    TherapeuticGoalPriority Priority,
    DateTime? ReviewDate);
