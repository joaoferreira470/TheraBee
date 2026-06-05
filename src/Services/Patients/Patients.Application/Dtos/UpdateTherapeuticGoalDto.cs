namespace Patients.Application.Dtos;

public record UpdateTherapeuticGoalDto(
    TherapeuticGoalType Type,
    string Description,
    string Area,
    TherapeuticGoalPriority Priority,
    DateTime? ReviewDate);
