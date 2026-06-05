namespace Patients.Application.Dtos;

public record CreateTherapeuticGoalDto(
    string Description,
    string Area,
    TherapeuticGoalPriority Priority,
    DateTime? ReviewDate);
