namespace Patients.Application.Dtos;

public record UpdateTherapeuticGoalDto(
    TherapeuticGoalType Type,
    string Description,
    TherapeuticGoalPriority Priority);
