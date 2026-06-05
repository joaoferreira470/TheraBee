namespace Patients.Application.Dtos;

public record CreateTherapeuticGoalDto(
    TherapeuticGoalType Type,
    Guid? ParentGoalId,
    string Description,
    string Area,
    TherapeuticGoalPriority Priority,
    DateTime? ReviewDate);
