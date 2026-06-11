namespace Patients.Application.Dtos;

public record CreateTherapeuticGoalDto(
    TherapeuticGoalType Type,
    string Description,
    TherapeuticGoalPriority Priority);
