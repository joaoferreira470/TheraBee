namespace Patients.Application.Dtos;

public record CreateTherapeuticGoalDto(
    TherapeuticGoalType Type,
    string Description,
    string Area,
    TherapeuticGoalPriority Priority);
